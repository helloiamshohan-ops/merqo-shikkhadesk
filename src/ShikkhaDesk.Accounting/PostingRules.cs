using ShikkhaDesk.Domain;

namespace ShikkhaDesk.Accounting;

/// <summary>
/// Resolved ids of the standard control accounts (seeded from the chart of
/// accounts). Posting rules reference these so the double-entry mappings in
/// ACCOUNTING-DESIGN.md are centralized and testable.
/// </summary>
public sealed record StandardAccounts(
    long CashOnHand,
    long StudentFeeReceivable,
    long StudentAdvanceCredit,
    long FeeIncome,
    long OtherIncome,
    long SupplierPayable,
    long SalaryPayable)
{
    // Bank accounts are dynamic (multiple), so they are passed per-transaction.
}

/// <summary>
/// Factory methods that build the standard balanced journal entries for each
/// business event (ACCOUNTING-DESIGN.md section 3). Each returns a DRAFT entry;
/// the caller assigns the journal number and posts it inside a transaction.
/// </summary>
public static class PostingRules
{
    /// <summary>3.1 Fee assessment (charge raised, not yet cash).</summary>
    public static JournalEntry FeeAssessment(StandardAccounts a, DateTime date, Money amount, long assessmentId)
    {
        var e = JournalEntry.Draft(date, "Fee assessment", nameof(FeeAssessment), assessmentId);
        e.AddDebit(a.StudentFeeReceivable, amount, "Receivable");
        e.AddCredit(a.FeeIncome, amount, "Fee income");
        return e;
    }

    /// <summary>3.2 Fee payment allocated to charges (settles receivable; not new income).</summary>
    public static JournalEntry FeePayment(StandardAccounts a, long depositAccountId, DateTime date, Money amount, long paymentId)
    {
        var e = JournalEntry.Draft(date, "Fee payment", nameof(FeePayment), paymentId);
        e.AddDebit(depositAccountId, amount, "Cash/Bank in");
        e.AddCredit(a.StudentFeeReceivable, amount, "Receivable settled");
        return e;
    }

    /// <summary>3.3a Advance payment received (cash in, obligation up).</summary>
    public static JournalEntry AdvanceReceived(StandardAccounts a, long depositAccountId, DateTime date, Money amount, long paymentId)
    {
        var e = JournalEntry.Draft(date, "Advance payment received", nameof(AdvanceReceived), paymentId);
        e.AddDebit(depositAccountId, amount, "Cash/Bank in");
        e.AddCredit(a.StudentAdvanceCredit, amount, "Advance credit (liability)");
        return e;
    }

    /// <summary>3.3b Advance allocated to a charge (moves obligation to settle receivable).</summary>
    public static JournalEntry AdvanceAllocated(StandardAccounts a, DateTime date, Money amount, long allocationId)
    {
        var e = JournalEntry.Draft(date, "Advance allocated to charge", nameof(AdvanceAllocated), allocationId);
        e.AddDebit(a.StudentAdvanceCredit, amount, "Advance used");
        e.AddCredit(a.StudentFeeReceivable, amount, "Receivable settled");
        return e;
    }

    /// <summary>3.5 Non-fee income.</summary>
    public static JournalEntry OtherIncome(StandardAccounts a, long incomeAccountId, long depositAccountId,
        DateTime date, Money amount, long incomeId)
    {
        var e = JournalEntry.Draft(date, "Other income", nameof(OtherIncome), incomeId);
        e.AddDebit(depositAccountId, amount);
        e.AddCredit(incomeAccountId, amount);
        return e;
    }

    /// <summary>3.6a Expense paid now from a cash/bank account.</summary>
    public static JournalEntry ExpensePaid(long expenseAccountId, long depositAccountId,
        DateTime date, Money amount, long expenseId)
    {
        var e = JournalEntry.Draft(date, "Expense (paid)", nameof(ExpensePaid), expenseId);
        e.AddDebit(expenseAccountId, amount);
        e.AddCredit(depositAccountId, amount);
        return e;
    }

    /// <summary>3.6b Expense on credit (bill recorded).</summary>
    public static JournalEntry ExpenseOnCredit(StandardAccounts a, long expenseAccountId,
        DateTime date, Money amount, long payableId)
    {
        var e = JournalEntry.Draft(date, "Expense on credit (bill)", nameof(ExpenseOnCredit), payableId);
        e.AddDebit(expenseAccountId, amount);
        e.AddCredit(a.SupplierPayable, amount);
        return e;
    }

    /// <summary>3.7 Transfer between institution-owned cash/bank accounts (not income/expense).</summary>
    public static JournalEntry Transfer(long fromAccountId, long toAccountId,
        DateTime date, Money amount, long transferId)
    {
        var e = JournalEntry.Draft(date, "Internal transfer", nameof(Transfer), transferId);
        e.AddDebit(toAccountId, amount, "To");
        e.AddCredit(fromAccountId, amount, "From");
        return e;
    }

    /// <summary>3.8 Payable settlement (no second expense).</summary>
    public static JournalEntry PayableSettlement(StandardAccounts a, long depositAccountId,
        DateTime date, Money amount, long settlementId)
    {
        var e = JournalEntry.Draft(date, "Payable settlement", nameof(PayableSettlement), settlementId);
        e.AddDebit(a.SupplierPayable, amount, "Payable reduced");
        e.AddCredit(depositAccountId, amount, "Cash/Bank out");
        return e;
    }

    /// <summary>3.9a Payroll accrual.</summary>
    public static JournalEntry PayrollAccrual(StandardAccounts a, long salaryExpenseAccountId,
        DateTime date, Money net, long payrollRunId)
    {
        var e = JournalEntry.Draft(date, "Payroll accrual", nameof(PayrollAccrual), payrollRunId);
        e.AddDebit(salaryExpenseAccountId, net, "Salary expense");
        e.AddCredit(a.SalaryPayable, net, "Salary payable");
        return e;
    }

    /// <summary>3.9b Payroll payment.</summary>
    public static JournalEntry PayrollPayment(StandardAccounts a, long depositAccountId,
        DateTime date, Money net, long payrollRunId)
    {
        var e = JournalEntry.Draft(date, "Payroll payment", nameof(PayrollPayment), payrollRunId);
        e.AddDebit(a.SalaryPayable, net, "Payable cleared");
        e.AddCredit(depositAccountId, net, "Cash/Bank out");
        return e;
    }
}
