using System.Security.Cryptography;
using System.Text;

namespace ShikkhaDesk.Licensing;

/// <summary>
/// Signs licenses with an ECDSA P-256 / SHA-256 PRIVATE key. Used ONLY by the
/// seller-side License Manager in a controlled environment. The private key is
/// NEVER embedded in the customer application (see LICENSE-DESIGN.md / SECURITY-PLAN).
/// Development and tests use clearly-labeled TEST keys.
/// </summary>
public sealed class LicenseSigner : IDisposable
{
    private readonly ECDsa _key;
    private readonly string _signingKeyId;

    /// <param name="privateKey">An ECDsa instance holding the private key.</param>
    /// <param name="signingKeyId">Identifier recorded in the license (distinguishes test vs production).</param>
    public LicenseSigner(ECDsa privateKey, string signingKeyId)
    {
        _key = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
        _signingKeyId = signingKeyId ?? throw new ArgumentNullException(nameof(signingKeyId));
    }

    /// <summary>Generate a fresh test key pair (development/tests only).</summary>
    public static (ECDsa PrivateKey, ECDsa PublicKey, string KeyId) CreateTestKey(string keyId = "test-key-2026")
    {
        var privateKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var publicKey = ECDsa.Create();
        publicKey.ImportParameters(ExportPublicOnly(privateKey));
        return (privateKey, publicKey, keyId);
    }

    private static ECParameters ExportPublicOnly(ECDsa key)
    {
        var p = key.ExportParameters(true);
        p.D = null; // drop the private scalar
        return p;
    }

    public SignedLicense Sign(LicensePayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        var canonical = Encoding.UTF8.GetBytes(payload.ToCanonicalJson());
        var sig = _key.SignData(canonical, HashAlgorithmName.SHA256);
        return new SignedLicense(payload.ToCanonicalJson(), Base64Url(sig), _signingKeyId);
    }

    internal static string Base64Url(byte[] bytes) =>
        Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    internal static byte[] FromBase64Url(string s)
    {
        var padded = s.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4) { case 2: padded += "=="; break; case 3: padded += "="; break; }
        return Convert.FromBase64String(padded);
    }

    public void Dispose() => _key.Dispose();
}
