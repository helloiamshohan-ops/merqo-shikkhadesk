# REQUIREMENTS — MERQO ShikkhaDesk

Derived from the master directive (PARTs 8–28). Each requirement has an ID and a
priority. **FR** = functional, **NFR** = non-functional, **SEC** = security,
**LIC** = licensing, **DATA** = data/backup. Status reflects design intent; nothing
here is "implemented/verified" until the build/test evidence exists
(see `PROJECT-STATE.md`).

Priorities: **MUST** (v1.0) · **SHOULD** (v1.0 if reliable) · **COULD** (later).

---

## Institution & setup
- **FR-SETUP-1 (MUST):** Guided Bangla first-run wizard: Welcome → institution
  details → academic setup → locale/digits → administrator → receipt settings →
  backup settings → host/client → license activation → review/finish. Validate
  inputs; prevent duplicate initialization; no public default admin password.
- **FR-SETUP-2 (MUST):** Later editing of all wizard settings via a Settings
  area, with audit history for sensitive changes.
- **FR-SETUP-3 (MUST):** Capture institution name, logo, address, phone, optional
  email, academic session, currency format, digit preference, receipt format &
  numbering, backup directory, host/client mode, LAN config, license file.

## Users, roles, security
- **FR-USER-1 (MUST):** Roles Admin / Accountant / Teacher by default; allow
  additional roles. Granular permissions: View, Create, Edit, Approve, Reverse,
  Print, Export, Restore, Manage users, Manage accounts, Manage devices.
- **SEC-USER-1 (MUST):** Enforce permissions in the **service/domain layer**, not
  by hiding UI. Secure password hashing (salted, e.g. PBKDF2/Argon2/bcrypt via a
  maintained library). Session management, password change, failed-login handling,
  user activation/deactivation.
- **SEC-USER-2 (MUST):** Audit history for financial and administrative actions.
  Ordinary users must not silently delete posted financial records or audit
  history (soft-delete + reversal only).

## Students & academics
- **FR-STU-1 (MUST):** Student registration with stable ID, name, guardian &
  contact, address, admission date, optional photo, academic info, course/batch
  enrollment, fee-plan assignment, scholarship/discount, status, notes.
- **FR-STU-2 (MUST):** Search/filter/sort/paginate; duplicate detection; archive &
  reactivate preserving history; student profile (academic, attendance, exam,
  payment, receipt histories; fee statement; outstanding & advance balances);
  export.
- **FR-ACA-1 (MUST):** Academic sessions, classes/grades, courses, subjects,
  batches, weekly schedules, teacher assignments, capacity limits (optional),
  admission, enrollment, batch/course transfer (preserving original records),
  withdrawal, reactivation, admission fee, roll numbers, batch registers.
- **FR-ATT-1 (MUST):** Daily/batch/session attendance, bulk entry, Present/Absent/
  Late/(Leave), makeup classes, correction with audit, teacher attendance,
  percentage, monthly reports, printable registers, export. Prevent duplicate
  attendance per student+batch+session; validate enrollment on the date; optimize
  for fast teacher entry.
- **FR-EXM-1 (MUST):** Exam setup, schedule, subject allocation, max/pass marks,
  marks entry, absent status, grading scale, GPA (where applicable), merit list,
  published/unpublished results (unpublished hidden from unauthorized users),
  correction, report cards, batch result sheets, print, export.

## Fees, payments, receipts, advance credit
- **FR-FEE-1 (MUST):** Fee types (monthly/course/admission/registration/exam/
  custom), installments, due dates, arrears, discounts, scholarships, approved
  adjustments, optional late fees, student-/batch-specific fees, fee-plan effective
  dates. Fee assessment + bulk generation + preview; **no duplicate charges on
  repeated bulk runs**; editing a plan must not silently change historical charges.
- **FR-PAY-1 (MUST):** Full/partial payment, manual & automatic allocation, cash,
  bank transfer, recorded bKash/Nagad (manual, with reference — **not** live
  integration), transaction reference, collection date, collector, receipt,
  refund, reversal, advance payment, student credit ledger.
- **FR-PAY-2 (MUST):** Idempotency: duplicate payment requests must not create
  duplicate payments; track received/allocated/refunded/available; credit cannot
  be allocated or refunded twice.

## Accounting (double-entry)
- **FR-ACC-1 (MUST):** Chart of accounts (Assets/Liabilities/Equity/Income/
  Expenses), configurable codes/names/categories/active, sensible starter accounts.
- **FR-ACC-2 (MUST):** Journal engine: every posted entry balances (debits =
  credits), atomic transactions, no unbalanced posting, journal number/date/
  description/debit+credit lines/source ref/posting status/approval/reversal/audit;
  no silent edit of posted entries.
- **FR-ACC-3 (MUST):** Income & expense with categories, voucher, register,
  approval, ledger integration; **student fees not double-counted as separate
  income**.
- **FR-ACC-4 (MUST):** Multiple cash/bank accounts, opening balances, receipts,
  payments, transfers, running balances, cashbook, bankbook, reconciliation, bank
  charges, account ledger; **transfers between own accounts are not income/expense**.
- **FR-ACC-5 (SHOULD):** Receivables (student fee receivables, other), payables
  (suppliers, bills, due dates, partial/full settlement), supplier ledger, aging;
  **payable settlement must not duplicate the expense**.
- **FR-ACC-6 (SHOULD):** Payroll: salary structure, period, basic, allowances,
  deductions, net, payable, payment, payslip/voucher, summary, ledger posting;
  no duplicate posting per employee+period without authorized adjustment.
- **FR-ACC-7 (MUST):** Statements: trial balance, P&L, balance sheet, cash-flow,
  general ledger, account ledger, cashbook, bankbook, income/expense reports,
  receivables, advance-credit liabilities, payables, payroll summary,
  reconciliation exceptions. Clearly defined periods; **no legal/VAT/statutory
  compliance claims without verified requirements**.

## Dashboard & reports
- **FR-DASH-1 (MUST):** Dashboard from real DB records: active students, new
  admissions, active batches, attendance, fee collection, outstanding, advance-
  credit liability, income, expenses, cash/bank balances, payables, payroll,
  recent receipts/transactions, backup/LAN/license status. Consistent periods;
  **collection ≠ profit; transfers ≠ income/expense**; respect permissions;
  drill-down.
- **FR-RPT-1 (MUST):** Report center; CSV import/export (Excel where practical)
  with template, mapping, preview, validation, duplicate detection, summary,
  error report, safe retry/rollback; data portability (customer can export own
  records).

## LAN / offline
- **NFR-LAN-1 (MUST):** Single-computer mode works alone; LAN mode = one
  authoritative host with SQLite on its disk; clients via authenticated app
  service/API. **Never** open the same SQLite file over SMB. Up to 5 registered
  computers. Host/client config, host address entry, connection test/status,
  auth, authz, request timeout, safe retry, reconnection, host-restart recovery,
  concurrent payments/receipts, idempotency, device registration, host migration.
- **NFR-LAN-2 (MUST):** A timed-out payment must be resolved by operation id/safe
  retry — never a duplicate; nothing reported successful before the authoritative
  DB transaction commits.
- **NFR-OFF-1 (MUST):** Core features work with no internet: students, fees,
  accounting, attendance, exams, printing, reports, backup, license verification.
  No mandatory online login/activation-refresh/cloud at startup.

## UI/UX
- **NFR-UI-1 (MUST):** Simple Bangla everywhere; consistent terminology; readable
  financial values; consistent icons; clear validation; keyboard navigation; focus
  indicators; display scaling; no needless animation/clutter; no tiny text on
  financial screens. Full 48-screen inventory in `UI-DESIGN-SPEC.md` — no
  placeholder screens.
- **NFR-UI-2 (MUST):** Global Bengali/English digit preference; Gregorian calendar;
  BDT default with configurable format; **money stored as integer minor units**,
  never binary floating point.

## Backup, data ownership
- **DATA-BKP-1 (MUST):** Automatic + manual local backup, history, configurable
  directory, retention, integrity checks, failure alerts; restore confirmation,
  pre-restore backup, recovery guide; never overwrite the only valid backup.
- **DATA-OWN-1 (MUST):** Customer data belongs to the institution; no secret
  transmission to MERQO; do not delete/encrypt data because a license is invalid;
  uninstall preserves data by default; provide an explicit authorized data-removal
  method.

## Licensing
- **LIC-1 (MUST):** Signed, institution-bound, offline-activatable license (see
  `LICENSE-DESIGN.md`). Verify with public key; reject altered/invalid/wrong-
  product/wrong-institution/malformed. Device limit (default 5) with registration,
  replacement, recovery, clear errors, no data deletion at limit.
- **LIC-2 (MUST):** Production private key never embedded in the customer app;
  test keys only in development; no production key without owner authorization
  (DEC-3). Perpetual use must not require periodic online checks.

## Non-functional
- **NFR-1 (MUST):** Windows desktop (.NET/WPF), offline-first, layered
  architecture, no business logic in UI handlers, maintained logging, DB
  migrations, appropriate installer tech.
- **NFR-2 (MUST):** Performance adequate for a coaching center; **no speed target
  claimed until measured**. Concurrency correctness over raw throughput.

## Explicitly out of scope for v1.0 (documented omission — DEC-5)
Parent/student portal, SMS/WhatsApp, payment-gateway APIs, biometric devices,
multi-branch, question banks/exam paper generation, certificates/ID cards,
library, inventory, transport, online courses/LMS. These are Optional/Future and
must not be advertised as available.
