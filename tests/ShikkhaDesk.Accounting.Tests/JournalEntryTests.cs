using ShikkhaDesk.Accounting;
using ShikkhaDesk.Domain;
using Xunit;

namespace ShikkhaDesk.Accounting.Tests;

public class JournalEntryTests
{
    private static JournalEntry Balanced() =>
        JournalEntry.Draft(new DateTime(2026, 10, 19), "Test")
            .AddDebit(1, Money.FromTaka(500))
            .AddCredit(2, Money.FromTaka(500));

    [Fact]
    public void Posting_BalancedEntry_Succeeds()
    {
        var e = Balanced();
        e.Post("JV-0001", postedBy: 1, DateTime.UtcNow);
        Assert.Equal(JournalStatus.Posted, e.Status);
        Assert.True(e.Balances);
    }

    [Fact]
    public void Posting_UnbalancedEntry_Throws()
    {
        var e = JournalEntry.Draft(new DateTime(2026, 10, 19), "Bad")
            .AddDebit(1, Money.FromTaka(500))
            .AddCredit(2, Money.FromTaka(400));
        Assert.Throws<InvalidOperationException>(() => e.Post("JV-0002", 1, DateTime.UtcNow));
    }

    [Fact]
    public void Line_CannotBeBothDebitAndCredit()
    {
        Assert.Throws<InvalidOperationException>(() =>
            JournalEntry.Draft(DateTime.Today, "x").AddDebit(1, Money.FromTaka(10))
                .AddCredit(1, Money.FromTaka(10)));
    }

    [Fact]
    public void PostedEntry_IsImmutable()
    {
        var e = Balanced();
        e.Post("JV-0003", 1, DateTime.UtcNow);
        Assert.Throws<InvalidOperationException>(() => e.AddDebit(3, Money.FromTaka(1)));
    }

    [Fact]
    public void Reversal_ReferencesSource_AndSwapsSides()
    {
        var e = Balanced();
        e.Post("JV-0004", 1, DateTime.UtcNow);
        var rev = e.CreateReversal(DateTime.Today, "error", 1, "JV-0005", DateTime.UtcNow);
        Assert.Equal(JournalStatus.Posted, rev.Status);
        Assert.Equal(e.TotalDebits, rev.TotalCredits);
        Assert.Equal(e.TotalCredits, rev.TotalDebits);
        Assert.True(rev.Balances);
    }
}
