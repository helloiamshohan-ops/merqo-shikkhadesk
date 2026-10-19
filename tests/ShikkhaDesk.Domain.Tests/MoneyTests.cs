using ShikkhaDesk.Domain;
using Xunit;

namespace ShikkhaDesk.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void FromTaka_RoundsToNearestPoisha()
    {
        Assert.Equal(125050, Money.FromTaka(1250.50m).Poisha);
        Assert.Equal(125001, Money.FromTaka(1250.005m).Poisha); // away-from-zero
    }

    [Fact]
    public void AdditionAndSubtraction_AreExactIntegerPoisha()
    {
        var a = Money.FromTaka(1000.10m);
        var b = Money.FromTaka(2000.20m);
        Assert.Equal(Money.FromTaka(3000.30m), a + b);
        Assert.Equal(Money.FromTaka(1000.10m), b - a);
    }

    [Fact]
    public void NoFloatingPointDrift_AcrossManyOperations()
    {
        // 0.1 taka added 1000 times must equal exactly 100 taka (no binary-float drift).
        var total = Money.Zero;
        for (int i = 0; i < 1000; i++) total += Money.FromTaka(0.10m);
        Assert.Equal(Money.FromTaka(100m), total);
    }

    [Fact]
    public void Comparisons_Work()
    {
        Assert.True(Money.FromTaka(5m) > Money.FromTaka(4.99m));
        Assert.True(Money.FromTaka(5m) == Money.FromPoisha(500));
        Assert.True(Money.FromTaka(0m).IsZero);
    }

    [Fact]
    public void Sum_ThrowsOnOverflow()
    {
        var items = new[] { Money.FromPoisha(long.MaxValue), Money.FromPoisha(1) };
        Assert.Throws<OverflowException>(() => Money.Sum(items));
    }
}
