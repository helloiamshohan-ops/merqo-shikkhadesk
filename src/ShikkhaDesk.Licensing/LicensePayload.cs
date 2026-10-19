using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ShikkhaDesk.Licensing;

/// <summary>
/// The signed license payload (LICENSE-DESIGN.md section 2). Versioned and
/// canonicalized deterministically so signatures are stable across builds.
/// </summary>
public sealed class LicensePayload
{
    public const string Format = "MERQO-ShikkhaDesk-License";
    public const string ProductId = "MERQO.ShikkhaDesk";
    public const int CurrentVersion = 1;

    public int Version { get; init; } = CurrentVersion;
    public required string FormatId { get; init; } = Format;
    public required string LicenseId { get; init; }
    public required string Product { get; init; } = ProductId;
    public required Guid InstitutionUid { get; init; }
    public required string InstitutionName { get; init; }
    public required string LicenseType { get; init; } = "Perpetual";
    public int MaxDevices { get; init; } = 5;
    public required string VersionEntitlement { get; init; }
    public required DateTime IssuedAtUtc { get; init; }
    public required string SigningKeyId { get; init; }

    private static string Iso(DateTime dt) =>
        dt.Kind == DateTimeKind.Utc ? dt.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)
                                    : dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

    /// <summary>
    /// Deterministic canonical JSON: keys sorted alphabetically, no insignificant
    /// whitespace, invariant culture. This exact byte sequence is what gets signed.
    /// </summary>
    public string ToCanonicalJson()
    {
        var sb = new StringBuilder();
        sb.Append('{');
        void Pair(string key, string rawValue, bool last)
        {
            sb.Append('"').Append(key).Append("\":").Append(rawValue);
            if (!last) sb.Append(',');
        }
        var issuedAt = "\"" + Iso(IssuedAtUtc) + "\"";
        var instUid = "\"" + InstitutionUid.ToString("D") + "\"";
        // Alphabetical key order: format, institutionName, institutionUid, issuedAt,
        // licenseId, licenseType, maxDevices, productId, signingKeyId, version, versionEntitlement
        Pair("format", JsonSerializer.Serialize(FormatId), false);
        Pair("institutionName", JsonSerializer.Serialize(InstitutionName), false);
        Pair("institutionUid", instUid, false);
        Pair("issuedAt", issuedAt, false);
        Pair("licenseId", JsonSerializer.Serialize(LicenseId), false);
        Pair("licenseType", JsonSerializer.Serialize(LicenseType), false);
        Pair("maxDevices", MaxDevices.ToString(CultureInfo.InvariantCulture), false);
        Pair("productId", JsonSerializer.Serialize(Product), false);
        Pair("signingKeyId", JsonSerializer.Serialize(SigningKeyId), false);
        Pair("version", Version.ToString(CultureInfo.InvariantCulture), false);
        Pair("versionEntitlement", JsonSerializer.Serialize(VersionEntitlement), true);
        sb.Append('}');
        return sb.ToString();
    }
}
