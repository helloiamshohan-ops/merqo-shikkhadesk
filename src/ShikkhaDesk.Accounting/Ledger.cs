using ShikkhaDesk.Domain;

namespace ShikkhaDesk.Accounting;

/// <summary>Per-account debit/credit totals and net balance.</summary>
public sealed record AccountBalance(long AccountId, Money Debits, Money Credits)
{
    public Money Net => Debits - Credits;
}

/// <summary>
/// Pure, in-memory ledger calculations over POSTED entries only. No I/O, so the
/// financial invariants are unit-testable without a database.
/// </summary>
public static class Ledger
{
    /// <summary>Posted entries within an optional inclusive date range.</summary>
    public static IEnumerable<JournalEntry> Posted(IEnumerable<JournalEntry> entries,
        DateTime? from = null, DateTime? to = null) =>
        entries.Where(e => e.Status == JournalStatus.Posted
            && (from is null || e.Date >= from.Value)
            && (to is null || e.Date <= to.Value));

    /// <summary>Net movement per account (debit-positive).</summary>
    public static Dictionary<long, AccountBalance> Balances(IEnumerable<JournalEntry> entries,
        DateTime? from = null, DateTime? to = null)
    {
        var map = new Dictionary<long, AccountBalance>();
        foreach (var e in Posted(entries, from, to))
        foreach (var l in e.Lines)
        {
            map.TryGetValue(l.AccountId, out var cur);
            var d = (cur?.Debits ?? Money.Zero) + l.Debit;
            var c = (cur?.Credits ?? Money.Zero) + l.Credit;
            map[l.AccountId] = new AccountBalance(l.AccountId, d, c);
        }
        return map;
    }

    /// <summary>Signed balance of one account given its category's normal side.</summary>
    public static Money SignedBalance(Dictionary<long, AccountBalance> balances, long accountId, Account account)
    {
        if (!balances.TryGetValue(accountId, out var b)) return Money.Zero;
        return account.IsDebitNormal ? b.Net : -b.Net;
    }

    /// <summary>
    /// Trial balance: total debits must equal total credits across all posted lines.
    /// Returns (totalDebits, totalCredits, balances) for an inclusive date range.
    /// </summary>
    public static (Money Debits, Money Credits, bool Balances) TrialBalance(
        IEnumerable<JournalEntry> entries, DateTime? from = null, DateTime? to = null)
    {
        Money dr = Money.Zero, cr = Money.Zero;
        foreach (var e in Posted(entries, from, to))
        {
            dr += e.TotalDebits;
            cr += e.TotalCredits;
        }
        return (dr, cr, dr == cr);
    }

    /// <summary>Profit = income - expenses over a range (both stored as signed nets).</summary>
    public static Money Profit(IEnumerable<JournalEntry> entries, IEnumerable<Account> accounts,
        DateTime from, DateTime to)
    {
        var bal = Balances(entries, from, to);
        Money income = Money.Zero, expense = Money.Zero;
        foreach (var a in accounts)
        {
            var signed = SignedBalance(bal, a.Id, a);
            if (a.Category == AccountCategory.Income) income += signed;
            else if (a.Category == AccountCategory.Expense) expense += signed;
        }
        return income - expense;
    }
}
