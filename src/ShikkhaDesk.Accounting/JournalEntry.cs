using ShikkhaDesk.Domain;

namespace ShikkhaDesk.Accounting;

/// <summary>
/// One line of a journal entry. A line is EITHER a debit OR a credit (never both,
/// never zero). Amounts are non-negative Money (poisha).
/// </summary>
public sealed class JournalLine : Entity
{
    public long JournalEntryId { get; internal set; }
    public long AccountId { get; private set; }
    public Money Debit { get; private set; }
    public Money Credit { get; private set; }
    public string? Memo { get; private set; }

    internal JournalLine() { }

    internal JournalLine(long accountId, Money debit, Money credit, string? memo = null)
    {
        if (accountId <= 0) throw new ArgumentException("Account required.", nameof(accountId));
        if (!debit.IsZero && !credit.IsZero)
            throw new InvalidOperationException("A journal line cannot be both debit and credit.");
        if (debit.IsNegative || credit.IsNegative)
            throw new InvalidOperationException("Journal line amounts must be non-negative.");
        if (debit.IsZero && credit.IsZero)
            throw new InvalidOperationException("A journal line must have a debit or credit amount.");
        AccountId = accountId;
        Debit = debit;
        Credit = credit;
        Memo = memo;
    }

    public bool IsDebit => !Debit.IsZero;
}

/// <summary>
/// A journal entry aggregate. Enforces the core accounting invariant:
/// total debits == total credits, checked atomically at Post() time.
/// Posted entries are immutable; corrections are Reversals that reference the source.
/// </summary>
public sealed class JournalEntry : Entity, IAggregateRoot
{
    private readonly List<JournalLine> _lines = new();

    public string JournalNo { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? SourceType { get; private set; }
    public long? SourceId { get; private set; }
    public JournalStatus Status { get; private set; } = JournalStatus.Draft;
    public long? PostedBy { get; private set; }
    public DateTime? PostedAt { get; private set; }
    public long? ReversesJournalId { get; private set; }
    public long? ReversedByJournalId { get; private set; }

    public IReadOnlyList<JournalLine> Lines => _lines;

    private JournalEntry() { }

    /// <summary>Create a draft entry (number assigned by the host when posting).</summary>
    public static JournalEntry Draft(DateTime date, string description,
        string? sourceType = null, long? sourceId = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description required.", nameof(description));
        return new JournalEntry { Date = date.Date, Description = description.Trim(), SourceType = sourceType, SourceId = sourceId };
    }

    public JournalEntry AddDebit(long accountId, Money amount, string? memo = null)
    {
        EnsureDraft();
        _lines.Add(new JournalLine(accountId, amount, Money.Zero, memo));
        return this;
    }

    public JournalEntry AddCredit(long accountId, Money amount, string? memo = null)
    {
        EnsureDraft();
        _lines.Add(new JournalLine(accountId, Money.Zero, amount, memo));
        return this;
    }

    public Money TotalDebits => Money.Sum(_lines.Where(l => l.IsDebit).Select(l => l.Debit));
    public Money TotalCredits => Money.Sum(_lines.Where(l => !l.IsDebit).Select(l => l.Credit));
    public bool Balances => TotalDebits == TotalCredits;

    /// <summary>
    /// Assign the journal number and post the entry. Throws if it does not balance
    /// or has fewer than two lines. Posting is the commit point for the entry.
    /// </summary>
    public void Post(string journalNo, long postedBy, DateTime postedAtUtc)
    {
        EnsureDraft();
        if (string.IsNullOrWhiteSpace(journalNo)) throw new ArgumentException("Journal number required.", nameof(journalNo));
        if (_lines.Count < 2) throw new InvalidOperationException("A journal entry needs at least two lines.");
        if (!Balances)
            throw new InvalidOperationException(
                $"Unbalanced journal: debits {TotalDebits} != credits {TotalCredits}.");
        JournalNo = journalNo.Trim();
        PostedBy = postedBy;
        PostedAt = postedAtUtc;
        Status = JournalStatus.Posted;
    }

    /// <summary>
    /// Create a reversing entry (swaps debits/credits) that references this posted
    /// entry. The original stays immutable; it is marked as reversed.
    /// </summary>
    public JournalEntry CreateReversal(DateTime date, string reason, long postedBy, string journalNo, DateTime postedAtUtc)
    {
        if (Status != JournalStatus.Posted)
            throw new InvalidOperationException("Only posted entries can be reversed.");
        var rev = new JournalEntry
        {
            Date = date.Date,
            Description = $"Reversal of {JournalNo}: {reason}",
            SourceType = SourceType,
            SourceId = SourceId,
            ReversesJournalId = Id == 0 ? null : Id,
        };
        foreach (var l in _lines)
        {
            if (l.IsDebit) rev.AddCredit(l.AccountId, l.Debit, l.Memo);
            else rev.AddDebit(l.AccountId, l.Credit, l.Memo);
        }
        rev.Post(journalNo, postedBy, postedAtUtc);
        ReversedByJournalId = rev.Id == 0 ? null : rev.Id;
        return rev;
    }

    private void EnsureDraft()
    {
        if (Status != JournalStatus.Draft)
            throw new InvalidOperationException("Posted/reversed entries are immutable.");
    }
}
