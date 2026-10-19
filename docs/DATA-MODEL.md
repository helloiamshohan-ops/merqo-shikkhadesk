# DATA MODEL — MERQO ShikkhaDesk

Normalized relational design for the authoritative SQLite database (PART 23).
Money is stored as **integer poisha** (`INTEGER`, 1 taka = 100 poisha) — never
floating point. All timestamps stored UTC; display localized. This is the design;
the implemented schema lives in `ShikkhaDesk.Persistence` (authored, not yet
built — see `PROJECT-STATE.md`).

Conventions: `Id` = INTEGER PK (except where a stable business key is required,
noted); `CreatedAt/UpdatedAt` UTC; soft-delete via `IsArchived`/`Status`; FKs
enforced; unique constraints where noted.

---

## Institution & identity
- **Institution** (singleton row): Id, InstitutionUid (immutable GUID — binds the
  license), Name, LogoPath, Address, Phone, Email?, AcademicSession, CurrencyCode,
  DigitPreference, ReceiptPrefix, ReceiptNextNumber, BackupDirectory, Mode
  (Single/LanHost/LanClient), HostAddress?, SchemaVersion, LicenseId?.
- **Device**: Id, InstitutionUid, DeviceUid, Name, Status (Registered/Replaced/
  Revoked), RegisteredAt, LastSeenAt, ReplacesDeviceId?.
- **License**: Id, LicenseId, InstitutionUid, LicensedName, MaxDevices,
  VersionEntitlement, LifetimeUse, IssuedAt, SigningKeyId, Signature, PayloadJson,
  Status. (Customer-side record of the activated license.)

## Users & permissions
- **User**: Id, Username (unique), PasswordHash, PasswordSalt, DisplayName, RoleId,
  IsActive, FailedLogins, LockedUntil?, LastLoginAt?, MustChangePassword.
- **Role**: Id, Name (Admin/Accountant/Teacher/custom), IsSystem.
- **Permission**: Id, Code (e.g. `fee.collect`, `journal.post`, `user.manage`,
  `device.manage`, `backup.restore`, `report.export`).
- **RolePermission**: RoleId, PermissionId (PK composite).
- **Session**: Id, UserId, Token, CreatedAt, ExpiresAt, RevokedAt?.
- **AuditLog**: Id, UserId?, Action, EntityType, EntityId, BeforeJson?, AfterJson?,
  At, DeviceId?. Append-only; never edited/deleted by ordinary users.

## Academic structure
- **AcademicSession**: Id, Name, StartDate, EndDate, IsCurrent.
- **Class/Grade**: Id, Name, Order.
- **Course**: Id, Name, Description?, IsActive.
- **Subject**: Id, Name, CourseId?, ClassId?.
- **Batch**: Id, Name, CourseId?, ClassId?, Capacity?, Status, SessionId.
- **BatchSchedule**: Id, BatchId, Weekday, StartTime, EndTime, SubjectId?, TeacherId?.
- **TeacherAssignment**: Id, StaffId, BatchId?, SubjectId?, SessionId.

## Students & admissions
- **Student**: Id, StudentUid (stable), Name, NameBn?, GuardianName, GuardianPhone,
  GuardianRelation, Phone?, Email?, Address?, DOB?, Gender?, PhotoPath?, Status
  (Active/Archived/Withdrawn), AdmittedAt, Notes?. Unique index on (Name,
  GuardianPhone) used for duplicate *detection* (warning, not hard block).
- **Admission**: Id, StudentId, AdmissionNo (unique), Date, SessionId, ClassId?,
  AdmissionFeePoisha, CreatedBy.
- **Enrollment**: Id, StudentId, BatchId, CourseId?, FromDate, ToDate?, Status
  (Active/Transferred/Withdrawn), RollNo?, SupersedesEnrollmentId? (history kept).
- **StudentDiscount**: Id, StudentId, Type (Discount/Scholarship), AmountPoisha or
  Percent, Reason, EffectiveFrom, EffectiveTo?, ApprovedBy.

## Attendance
- **AttendanceSession**: Id, BatchId, Date, SubjectId?, TeacherId?, Type
  (Regular/Makeup), Notes?.
- **AttendanceRecord**: Id, AttendanceSessionId, StudentId, Status
  (Present/Absent/Late/Leave), MarkedBy, MarkedAt, CorrectedBy?, CorrectionNote?.
  Unique (AttendanceSessionId, StudentId).

## Exams & results
- **Exam**: Id, Name, SessionId, StartDate?, EndDate?, Status (Draft/Published).
- **ExamSubject**: Id, ExamId, SubjectId, MaxMarks, PassMarks, ExamDate?.
- **GradingScale**: Id, Name; **GradeBand**: Id, GradingScaleId, Grade, MinPercent,
  MaxPercent, GradePoint?.
- **Mark**: Id, ExamSubjectId, StudentId, MarksObtained?, IsAbsent, EnteredBy,
  EnteredAt, CorrectedBy?, CorrectionNote?. Unique (ExamSubjectId, StudentId).

## Fees
- **FeeType**: Id, Name, IsRecurring, IsActive.
- **FeePlan**: Id, Name, EffectiveFrom, EffectiveTo?, Status. (Editing creates a
  new version; historical charges keep their plan snapshot.)
- **FeePlanItem**: Id, FeePlanId, FeeTypeId, AmountPoisha, AppliesTo
  (Batch/Class/Student), BatchId?/ClassId?/StudentId?.
- **FeeAssessment**: Id, StudentId, Period (e.g. 2026-10), AssessedOn, TotalPoisha,
  Status (Draft/Posted), PlanSnapshotJson, CreatedBy. Unique (StudentId, Period,
  FeePlanVersion) to prevent duplicate bulk charges.
- **Charge**: Id, FeeAssessmentId, FeeTypeId, AmountPoisha, DueDate?, AccountId
  (receivable account), PostedJournalId?.

## Payments, allocations, credit, receipts
- **Payment**: Id, PaymentUid (idempotency), StudentId?, AmountPoisha, Method
  (Cash/Bank/Bkash/Nagad/Other), Reference?, CollectedOn, CollectorUserId,
  AccountId (cash/bank), Status, PostedJournalId?. Unique (PaymentUid).
- **Allocation**: Id, PaymentId, ChargeId, AmountPoisha. Sum(allocation) ≤
  payment.Amount − refunded; a charge cannot be over-allocated.
- **StudentCredit**: Id, StudentId, AmountPoisha (available advance), running
  balance. Debit on advance/unapplied payment; credit when allocated/refunded.
- **Receipt**: Id, ReceiptNo (unique, host-allocated), PaymentId, IssuedOn,
  InstitutionSnapshot, AmountPoisha, PrintedCount.
- **Refund**: Id, PaymentId or AllocationId, AmountPoisha, Reason, RefundedOn,
  AccountId, PostedJournalId?. Cannot exceed refundable amount; cannot double-refund.

## Accounting
- **Account** (Chart of Accounts): Id, Code (unique), Name, Category
  (Asset/Liability/Equity/Income/Expense), ParentId?, IsActive, IsSystemAccount.
- **AccountingPeriod**: Id, Name, StartDate, EndDate, Status (Open/Closed).
- **JournalEntry**: Id, JournalNo (unique), Date, Description, SourceType, SourceId,
  Status (Draft/Posted/Reversed), PostedBy, PostedAt, ReversesJournalId?.
- **JournalLine**: Id, JournalEntryId, AccountId, DebitPoisha, CreditPoisha,
  Memo?. Invariant: Σdebits = Σcredits per entry; both not > 0 on one line.
- **IncomeEntry**: Id, Date, CategoryId, AmountPoisha, AccountId (cash/bank),
  Description, SupplierRef?, EnteredBy, ApprovedBy?, VoucherNo, PostedJournalId?.
- **ExpenseEntry**: Id, Date, CategoryId, AmountPoisha, AccountId, Description,
  SupplierId?, EnteredBy, ApprovedBy?, VoucherNo, PostedJournalId?.
- **FinancialAccount** (cash/bank): Id, Name, Type (Cash/Bank), OpeningBalancePoisha,
  AccountId (linked CoA), IsActive.
- **Transfer**: Id, FromAccountId, ToAccountId, AmountPoisha, Date, Note,
  PostedJournalId?. Not income/expense.
- **Supplier**: Id, Name, Phone?, Address?, OpeningPayablePoisha, IsActive.
- **Payable** (bill): Id, SupplierId, BillNo, Date, DueDate?, AmountPoisha,
  Status, ExpenseAccountId, PostedJournalId?.
- **PayableSettlement**: Id, PayableId, AmountPoisha, Date, AccountId,
  PostedJournalId?. Prevents double-counting the expense.
- **PayrollRun**: Id, Period, StaffId, BasicPoisha, AllowancesPoisha,
  DeductionsPoisha, NetPoisha, PaidOn?, Status, PayslipNo, PostedJournalId?.
  Unique (StaffId, Period) absent an authorized adjustment.
- **Staff**: Id, Name, Designation, Phone?, JoinDate, LeaveDate?, Status,
  SalaryStructureJson. Salary fields access-restricted (Accountant/Admin only).

## Backup & settings
- **BackupRecord**: Id, FilePath, SizeBytes, CreatedAt, Type (Auto/Manual/PreRestore),
  IntegrityOk, Checksum, Note?.
- **Setting**: Id, Key (unique), Value, UpdatedAt. (Feature flags, receipt format.)

---

## Key integrity rules (enforced in DB + domain)
1. JournalEntry lines must balance (Σ debit = Σ credit) — enforced at post time in
   a transaction.
2. Allocation sum per payment ≤ (payment amount − refunded); charge allocation ≤
   charge amount.
3. Student credit available ≥ 0 after each operation; no double allocation/refund.
4. Transfers never touch Income/Expense accounts.
5. Payable settlement does not create a second expense.
6. Posted journals are immutable; corrections are reversals referencing the source.
7. Enrollment transfers insert a new row and supersede (never overwrite) the old.
8. Fee assessment unique per (student, period, plan version) → no duplicate bulk
   charges; plan edits create new versions, leaving history intact.
