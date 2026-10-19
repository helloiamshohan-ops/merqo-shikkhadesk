using ShikkhaDesk.Accounting;
using ShikkhaDesk.Domain;
using Xunit;

namespace ShikkhaDesk.Accounting.Tests;

/// <summary>
/// Financial invariants (ACCOUNTING-DESIGN.md section 7 / PART 17.8), verified
/// in-memory against the posting rules and ledger. These are the tests that must
/// pass (on a real .NET runtime) before any financial claim is made.
/// </summary>
public class FinancialInvariantTests
{
    // Well-known account ids for the scenario.
    private const long Cash = 100;
    private const long Bank = 101;
    private const long FeeReceivable = 200;
    private const long AdvanceCredit = 201;
    private const long FeeIncome = 300;
    private const long RentExpense = 400;
    private const long SupplierPayable = 500;

    private static readonly StandardAccounts Std = new(
        CashOnHand: Cash,
        StudentFeeReceivable: FeeReceivable,
        StudentAdvanceCredit: AdvanceCredit,
        FeeIncome: FeeIncome,
        OtherIncome: 301,
        SupplierPayable: SupplierPayable,
        SalaryPayable: 501);

    private static Account Acct(long id, AccountCategory cat) => new($"A{id}", $"Account {id}", cat) { Id = id };

    private static List<Account> Chart() => new()
    {
        Acct(Cash, AccountCategory.Asset),
        Acct(Bank, AccountCategory.Asset),
        Acct(FeeReceivable, AccountCategory.Asset),
        Acct(AdvanceCredit, AccountCategory.Liability),
        Acct(FeeIncome, AccountCategory.Income),
        Acct(RentExpense, AccountCategory.Expense),
        Acct(SupplierPayable, AccountCategory.Liability),
    };

    private static JournalEntry Post(JournalEntry e, string no) { e.Post(no, 1, DateTime.UtcNow); return e; }

    [Fact]
    public void All_PostingRules_Produce_BalancedEntries()
    {
        JournalEntry[] entries =
        {
            Post(PostingRules.FeeAssessment(Std, DateTime.Today, Money.FromTaka(1000), 1), "JV-1"),
            Post(PostingRules.FeePayment(Std, Cash, DateTime.Today, Money.FromTaka(600), 2), "JV-2"),
            Post(PostingRules.AdvanceReceived(Std, Bank, DateTime.Today, Money.FromTaka(400), 3), "JV-3"),
            Post(PostingRules.AdvanceAllocated(Std, DateTime.Today, Money.FromTaka(400), 4), "JV-4"),
            Post(PostingRules.ExpenseOnCredit(Std, RentExpense, DateTime.Today, Money.FromTaka(200), 5), "JV-5"),
            Post(PostingRules.PayableSettlement(Std, Cash, DateTime.Today, Money.FromTaka(200), 6), "JV-6"),
            Post(PostingRules.Transfer(Bank, Cash, DateTime.Today, Money.FromTaka(50), 7), "JV-7"),
        };
        foreach (var e in entries) Assert.True(e.Balances, $"{e.JournalNo} must balance");

        var tb = Ledger.TrialBalance(entries);
        Assert.True(tb.Balances);               // invariant 1 / trial balance
        Assert.Equal(tb.Debits, tb.Credits);
    }

    [Fact]
    public void Transfer_DoesNotTouchIncomeOrExpense()
    {
        var e = Post(PostingRules.Transfer(Bank, Cash, DateTime.Today, Money.FromTaka(50), 7), "JV-7");
        Assert.DoesNotContain(e.Lines, l => l.AccountId == FeeIncome);
        Assert.DoesNotContain(e.Lines, l => l.AccountId == RentExpense);
    }

    [Fact]
    public void Fees_AreNotDoubleCounted_AsIncome()
    {
        // Assess 1000 (income recognized once) then collect 1000 (settles receivable).
        var entries = new List<JournalEntry>
        {
            Post(PostingRules.FeeAssessment(Std, DateTime.Today, Money.FromTaka(1000), 1), "JV-1"),
            Post(PostingRules.FeePayment(Std, Cash, DateTime.Today, Money.FromTaka(1000), 2), "JV-2"),
        };
        var chart = Chart();
        var bal = Ledger.Balances(entries);
        var income = Ledger.SignedBalance(bal, FeeIncome, chart.First(a => a.Id == FeeIncome));
        Assert.Equal(Money.FromTaka(1000), income); // recognized exactly once
    }

    [Fact]
    public void StudentReceivable_Reconciles_ToAssessedMinusCollected()
    {
        var entries = new List<JournalEntry>
        {
            Post(PostingRules.FeeAssessment(Std, DateTime.Today, Money.FromTaka(1000), 1), "JV-1"),
            Post(PostingRules.FeePayment(Std, Cash, DateTime.Today, Money.FromTaka(600), 2), "JV-2"),
        };
        var chart = Chart();
        var bal = Ledger.Balances(entries);
        var receivable = Ledger.SignedBalance(bal, FeeReceivable, chart.First(a => a.Id == FeeReceivable));
        Assert.Equal(Money.FromTaka(400), receivable); // 1000 - 600
    }

    [Fact]
    public void AdvanceCredit_IsLiability_NotIncome()
    {
        var entries = new List<JournalEntry>
        {
            Post(PostingRules.AdvanceReceived(Std, Cash, DateTime.Today, Money.FromTaka(300), 3), "JV-3"),
        };
        var chart = Chart();
        var bal = Ledger.Balances(entries);
        // Advance credit balance is a liability (credit-normal -> positive signed).
        var adv = Ledger.SignedBalance(bal, AdvanceCredit, chart.First(a => a.Id == AdvanceCredit));
        Assert.Equal(Money.FromTaka(300), adv);
        // No income recognized yet.
        var income = Ledger.SignedBalance(bal, FeeIncome, chart.First(a => a.Id == FeeIncome));
        Assert.True(income.IsZero);
    }

    [Fact]
    public void Profit_EqualsIncomeMinusExpense()
    {
        var entries = new List<JournalEntry>
        {
            Post(PostingRules.OtherIncome(Std, 301, Cash, DateTime.Today, Money.FromTaka(5000), 10), "JV-10"),
            Post(PostingRules.ExpensePaid(RentExpense, Cash, DateTime.Today, Money.FromTaka(1200), 11), "JV-11"),
        };
        var profit = Ledger.Profit(entries, Chart(), DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));
        Assert.Equal(Money.FromTaka(3800), profit);
    }

    [Fact]
    public void PayableSettlement_DoesNotCreateSecondExpense()
    {
        var entries = new List<JournalEntry>
        {
            Post(PostingRules.ExpenseOnCredit(Std, RentExpense, DateTime.Today, Money.FromTaka(200), 5), "JV-5"),
            Post(PostingRules.PayableSettlement(Std, Cash, DateTime.Today, Money.FromTaka(200), 6), "JV-6"),
        };
        var chart = Chart();
        var bal = Ledger.Balances(entries);
        var rent = Ledger.SignedBalance(bal, RentExpense, chart.First(a => a.Id == RentExpense));
        Assert.Equal(Money.FromTaka(200), rent); // expense counted exactly once
    }
}
