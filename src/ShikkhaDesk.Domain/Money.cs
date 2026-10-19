namespace ShikkhaDesk.Domain;

/// <summary>
/// Monetary amount stored as integer poisha (1 taka = 100 poisha).
/// Authoritative representation of money — never a binary floating-point value.
/// Display formatting (Bengali/English digits, currency symbol) happens only at
/// the presentation/reporting boundary; storage and math always use this type.
/// </summary>
public readonly struct Money : IEquatable<Money>, IComparable<Money>
{
    /// <summary>Poisha per taka.</summary>
    public const long PoishaPerTaka = 100;

    /// <summary>The amount in poisha (may be negative for corrections/adjustments).</summary>
    public long Poisha { get; }

    private Money(long poisha) => Poisha = poisha;

    /// <summary>Create from poisha.</summary>
    public static Money FromPoisha(long poisha) => new(poisha);

    /// <summary>Create from taka (e.g. 1250.50m taka). Rounds to nearest poisha.</summary>
    public static Money FromTaka(decimal taka) =>
        new((long)Math.Round(taka * PoishaPerTaka, MidpointRounding.AwayFromZero));

    /// <summary>Zero.</summary>
    public static Money Zero => new(0);

    /// <summary>Whole taka value as a decimal (for display/export only).</summary>
    public decimal ToTaka() => Poisha / (decimal)PoishaPerTaka;

    public bool IsZero => Poisha == 0;
    public bool IsNegative => Poisha < 0;
    public bool IsPositive => Poisha > 0;

    public static Money operator +(Money a, Money b) => new(a.Poisha + b.Poisha);
    public static Money operator -(Money a, Money b) => new(a.Poisha - b.Poisha);
    public static Money operator -(Money a) => new(-a.Poisha);

    /// <summary>Multiply by an integer quantity (e.g. monthly fee × months).</summary>
    public static Money operator *(Money a, int quantity)
    {
        if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        return new Money(a.Poisha * quantity);
    }

    public static bool operator ==(Money a, Money b) => a.Poisha == b.Poisha;
    public static bool operator !=(Money a, Money b) => a.Poisha != b.Poisha;
    public static bool operator <(Money a, Money b) => a.Poisha < b.Poisha;
    public static bool operator >(Money a, Money b) => a.Poisha > b.Poisha;
    public static bool operator <=(Money a, Money b) => a.Poisha <= b.Poisha;
    public static bool operator >=(Money a, Money b) => a.Poisha >= b.Poisha;

    /// <summary>Absolute value.</summary>
    public Money Abs() => new(Math.Abs(Poisha));

    /// <summary>Min/Max helpers for allocation bounds.</summary>
    public static Money Min(Money a, Money b) => a.Poisha <= b.Poisha ? a : b;
    public static Money Max(Money a, Money b) => a.Poisha >= b.Poisha ? a : b;

    /// <summary>Sum a sequence; throws on overflow (financial safety).</summary>
    public static Money Sum(IEnumerable<Money> items)
    {
        long total = 0;
        foreach (var m in items)
            checked { total += m.Poisha; }
        return new Money(total);
    }

    public bool Equals(Money other) => Poisha == other.Poisha;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => Poisha.GetHashCode();
    public int CompareTo(Money other) => Poisha.CompareTo(other.Poisha);

    public override string ToString() => $"{ToTaka():0.00} BDT";
}
