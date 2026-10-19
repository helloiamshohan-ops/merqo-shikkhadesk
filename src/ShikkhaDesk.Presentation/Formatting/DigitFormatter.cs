using System.Globalization;
using System.Text;
using ShikkhaDesk.Domain;

namespace ShikkhaDesk.Presentation.Formatting;

/// <summary>
/// Centralized display formatting (PART 7.1/7.2). Storage and math always use
/// canonical English-digit values; only presentation is localized. The global
/// digit preference (Bengali/English) and currency are applied here.
/// </summary>
public static class DigitFormatter
{
    private const string BengaliDigits = "০১২৩৪৫৬৭৮৯";

    /// <summary>Convert ASCII digits in a string to Bengali digits.</summary>
    public static string ToBengaliDigits(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
            sb.Append(ch is >= '0' and <= '9' ? BengaliDigits[ch - '0'] : ch);
        return sb.ToString();
    }

    /// <summary>Apply the user's digit preference to an already-formatted string.</summary>
    public static string ApplyDigits(string formatted, DigitPreference preference) =>
        preference == DigitPreference.Bengali ? ToBengaliDigits(formatted) : formatted;

    /// <summary>Format money: symbol + thousands-separated amount, honoring digit preference.</summary>
    public static string Money(long poisha, DigitPreference preference, string currencySymbol = "৳")
    {
        var taka = poisha / 100m;
        var s = $"{currencySymbol} {taka.ToString("#,##0.00", CultureInfo.InvariantCulture)}";
        return ApplyDigits(s, preference);
    }

    /// <summary>Format an integer (ids, counts) honoring digit preference.</summary>
    public static string Number(long value, DigitPreference preference) =>
        ApplyDigits(value.ToString(CultureInfo.InvariantCulture), preference);

    /// <summary>Gregorian date (e.g. 2026-10-19) honoring digit preference.</summary>
    public static string Date(DateTime date, DigitPreference preference) =>
        ApplyDigits(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), preference);
}
