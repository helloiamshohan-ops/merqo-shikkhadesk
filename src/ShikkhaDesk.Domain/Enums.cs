namespace ShikkhaDesk.Domain;

/// <summary>Granular permission codes (enforced in the service/domain layer).</summary>
public enum Permission
{
    // Students & academics
    StudentView, StudentCreate, StudentEdit,
    AdmissionManage, AcademicManage,
    AttendanceView, AttendanceTake, AttendanceCorrect,
    ExamManage, MarksEnter, ResultPublish,
    // Fees & payments
    FeePlanManage, FeeAssess, FeeCollect, FeeAdjust,
    AdvanceCreditManage, RefundApprove, ReceiptPrint,
    // Accounting
    AccountManage, IncomeEnter, ExpenseEnter, JournalPost, JournalReverse,
    CashBankManage, TransferCreate, PayableManage, PayrollManage,
    ReportView, ReportExport,
    // Administration
    UserManage, RoleManage, AuditView,
    BackupCreate, BackupRestore, SettingsManage,
    DeviceManage, LicenseActivate
}

/// <summary>Built-in role names (additional custom roles are allowed).</summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Accountant = "Accountant";
    public const string Teacher = "Teacher";
}

/// <summary>Account category in the chart of accounts.</summary>
public enum AccountCategory { Asset, Liability, Equity, Income, Expense }

/// <summary>Posting status of a journal entry.</summary>
public enum JournalStatus { Draft, Posted, Reversed }

/// <summary>Payment method (bKash/Nagad are recorded manually, not live-integrated).</summary>
public enum PaymentMethod { Cash, BankTransfer, Bkash, Nagad, Other }

/// <summary>Lifecycle status shared by several records.</summary>
public enum RecordStatus { Active, Archived, Withdrawn }

/// <summary>Application run mode.</summary>
public enum RunMode { Single, LanHost, LanClient }

/// <summary>Attendance status.</summary>
public enum AttendanceStatus { Present, Absent, Late, Leave }
