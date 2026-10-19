using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ShikkhaDesk.Licensing;

/// <summary>Outcome of verifying a license.</summary>
public enum LicenseStatus
{
    Valid,
    Malformed,
    UnknownSigningKey,
    InvalidSignature,
    WrongProduct,
    WrongInstitution,
    UnsupportedVersion,
    DeviceLimitExceeded
}

/// <summary>Detailed verification result.</summary>
public sealed record LicenseVerificationResult(
    LicenseStatus Status,
    string Message,
    LicensePayload? Payload = null)
{
    public bool IsValid => Status == LicenseStatus.Valid;
}

/// <summary>
/// Verifies signed licenses using a PUBLIC key only. Runs in the customer app and
/// works fully offline. Enforces institution binding and entitlement checks
/// (LICENSE-DESIGN.md section 3). No private key material is present here.
/// </summary>
public sealed class LicenseVerifier
{
    private readonly ECParameters _publicKey;
    private readonly string _expectedSigningKeyId;

    /// <param name="publicKey">Public-only EC parameters (safe to embed in the customer app).</param>
    /// <param name="expectedSigningKeyId">The signing-key id this build trusts.</param>
    public LicenseVerifier(ECParameters publicKey, string expectedSigningKeyId)
    {
        if (publicKey.D is not null)
            throw new ArgumentException("Public key must not contain a private scalar.", nameof(publicKey));
        _publicKey = publicKey;
        _expectedSigningKeyId = expectedSigningKeyId;
    }

    public LicenseVerificationResult Verify(SignedLicense? license, Guid institutionUid,
        string appVersion, int registeredDeviceCount)
    {
        if (license is null)
            return new(LicenseStatus.Malformed, "লাইসেন্স ফাইল পাওয়া যায়নি।");

        if (!string.Equals(license.SigningKeyId, _expectedSigningKeyId, StringComparison.Ordinal))
            return new(LicenseStatus.UnknownSigningKey, "অজানা স্বাক্ষর কী।");

        var payload = TryParsePayload(license.Payload);
        if (payload is null)
            return new(LicenseStatus.Malformed, "লাইসেন্স ফাইলের গঠন সঠিক নয়।");

        // 1. Signature over the EXACT payload bytes.
        byte[] payloadBytes = Encoding.UTF8.GetBytes(license.Payload);
        byte[] signature;
        try { signature = LicenseSigner.FromBase64Url(license.Signature); }
        catch (FormatException) { return new(LicenseStatus.Malformed, "স্বাক্ষর সঠিক নয়।"); }

        using var ecdsa = ECDsa.Create(_publicKey);
        if (!ecdsa.VerifyData(payloadBytes, signature, HashAlgorithmName.SHA256))
            return new(LicenseStatus.InvalidSignature, "লাইসেন্স যাচাই ব্যর্থ (স্বাক্ষর মিলছে না)।");

        // 2. Product binding.
        if (!string.Equals(payload.Product, LicensePayload.ProductId, StringComparison.Ordinal))
            return new(LicenseStatus.WrongProduct, "ভুল পণ্য (product)।");

        // 3. Institution binding (immutable uid — a name change does not transfer).
        if (payload.InstitutionUid != institutionUid)
            return new(LicenseStatus.WrongInstitution, "লাইসেন্স অন্য প্রতিষ্ঠানের জন্য।");

        // 4. Version entitlement.
        if (!VersionEntitled(payload.VersionEntitlement, appVersion))
            return new(LicenseStatus.UnsupportedVersion, "এই সংস্করণটি সাপোর্ট করে না।");

        // 5. Device limit.
        if (registeredDeviceCount > payload.MaxDevices)
            return new(LicenseStatus.DeviceLimitExceeded,
                $"কম্পিউটারের সীমা ({payload.MaxDevices}) ছাড়িয়ে গেছে।");

        return new(LicenseStatus.Valid, "লাইসেন্স যাচাই সফল।", payload);
    }

    private static LicensePayload? TryParsePayload(string payload)
    {
        try
        {
            using var doc = JsonDocument.Parse(payload);
            var r = doc.RootElement;
            string Str(string k) => r.TryGetProperty(k, out var v) ? v.GetString() ?? "" : "";
            int Int(string k) => r.TryGetProperty(k, out var v) && v.TryGetInt32(out var i) ? i : 0;
            if (Str("format") != LicensePayload.Format)
                return null;
            if (!Guid.TryParse(Str("institutionUid"), out var uid))
                return null;
            if (!DateTime.TryParse(Str("issuedAt"), CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var issued))
                return null;
            return new LicensePayload
            {
                FormatId = Str("format"),
                LicenseId = Str("licenseId"),
                Product = Str("productId"),
                InstitutionUid = uid,
                InstitutionName = Str("institutionName"),
                LicenseType = Str("licenseType"),
                MaxDevices = Int("maxDevices"),
                VersionEntitlement = Str("versionEntitlement"),
                IssuedAtUtc = issued,
                SigningKeyId = Str("signingKeyId"),
                Version = Int("version")
            };
        }
        catch (JsonException) { return null; }
    }

    /// <summary>Entitlement "1.x" means any 1.* version; "1.0" means exactly 1.0.</summary>
    private static bool VersionEntitled(string entitlement, string appVersion)
    {
        if (string.IsNullOrWhiteSpace(entitlement)) return false;
        var ent = entitlement.Trim();
        if (ent.EndsWith(".x", StringComparison.OrdinalIgnoreCase))
        {
            var major = ent[..^2];
            var appMajor = appVersion.Split('.')[0];
            return string.Equals(major, appMajor, StringComparison.Ordinal);
        }
        return string.Equals(ent, appVersion, StringComparison.Ordinal);
    }
}
