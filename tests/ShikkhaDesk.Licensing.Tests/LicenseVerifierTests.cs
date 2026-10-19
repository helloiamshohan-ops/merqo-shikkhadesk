using System.Security.Cryptography;
using ShikkhaDesk.Licensing;
using Xunit;

namespace ShikkhaDesk.Licensing.Tests;

public class LicenseVerifierTests
{
    private static readonly Guid InstitutionUid = Guid.Parse("11111111-2222-3333-4444-555555555555");

    private static (ECDsa Priv, ECParameters Pub, string KeyId) Keys()
    {
        var (priv, _, keyId) = LicenseSigner.CreateTestKey("test-key-2026");
        var pub = priv.ExportParameters(true);
        pub.D = null;
        return (priv, pub, keyId);
    }

    private static LicensePayload Payload(string keyId, Guid? inst = null, string product = LicensePayload.ProductId) => new()
    {
        FormatId = LicensePayload.Format,
        LicenseId = "LIC-TEST0001",
        Product = product,
        InstitutionUid = inst ?? InstitutionUid,
        InstitutionName = "Test Coaching",
        LicenseType = "Perpetual",
        MaxDevices = 5,
        VersionEntitlement = "1.x",
        IssuedAtUtc = new DateTime(2026, 10, 19, 0, 0, 0, DateTimeKind.Utc),
        SigningKeyId = keyId
    };

    private static LicenseVerifier Verifier(ECParameters pub, string keyId = "test-key-2026") =>
        new(pub, keyId);

    [Fact]
    public void ValidLicense_VerifiesOffline()
    {
        var (priv, pub, keyId) = Keys();
        using var signer = new LicenseSigner(priv, keyId);
        var signed = signer.Sign(Payload(keyId));
        var v = Verifier(pub, keyId);
        var r = v.Verify(signed, InstitutionUid, appVersion: "1.0.0", registeredDeviceCount: 2);
        Assert.True(r.IsValid, r.Message);
    }

    [Fact]
    public void TamperedPayload_IsRejected()
    {
        var (priv, pub, keyId) = Keys();
        using var signer = new LicenseSigner(priv, keyId);
        var signed = signer.Sign(Payload(keyId));
        var tampered = new SignedLicense(
            signed.Payload.Replace("Test Coaching", "Hacked Coaching"), signed.Signature, signed.SigningKeyId);
        var r = Verifier(pub, keyId).Verify(tampered, InstitutionUid, "1.0.0", 1);
        Assert.Equal(LicenseStatus.InvalidSignature, r.Status);
    }

    [Fact]
    public void WrongInstitution_IsRejected()
    {
        var (priv, pub, keyId) = Keys();
        using var signer = new LicenseSigner(priv, keyId);
        var signed = signer.Sign(Payload(keyId, inst: InstitutionUid));
        var otherInstitution = Guid.NewGuid();
        var r = Verifier(pub, keyId).Verify(signed, otherInstitution, "1.0.0", 1);
        Assert.Equal(LicenseStatus.WrongInstitution, r.Status);
    }

    [Fact]
    public void WrongProduct_IsRejected()
    {
        var (priv, pub, keyId) = Keys();
        using var signer = new LicenseSigner(priv, keyId);
        var signed = signer.Sign(Payload(keyId, product: "SomeOther.Product"));
        var r = Verifier(pub, keyId).Verify(signed, InstitutionUid, "1.0.0", 1);
        Assert.Equal(LicenseStatus.WrongProduct, r.Status);
    }

    [Fact]
    public void DeviceLimit_IsEnforced()
    {
        var (priv, pub, keyId) = Keys();
        using var signer = new LicenseSigner(priv, keyId);
        var signed = signer.Sign(Payload(keyId)); // MaxDevices = 5
        var r = Verifier(pub, keyId).Verify(signed, InstitutionUid, "1.0.0", registeredDeviceCount: 6);
        Assert.Equal(LicenseStatus.DeviceLimitExceeded, r.Status);
    }

    [Fact]
    public void VersionEntitlement_1x_Allows1Any_ButNot2()
    {
        var (priv, pub, keyId) = Keys();
        using var signer = new LicenseSigner(priv, keyId);
        var signed = signer.Sign(Payload(keyId)); // "1.x"
        Assert.True(Verifier(pub, keyId).Verify(signed, InstitutionUid, "1.7.3", 1).IsValid);
        Assert.Equal(LicenseStatus.UnsupportedVersion,
            Verifier(pub, keyId).Verify(signed, InstitutionUid, "2.0.0", 1).Status);
    }

    [Fact]
    public void MalformedFile_IsRejected_NotThrown()
    {
        Assert.Null(SignedLicense.TryParse("{ not json"));
        Assert.Null(SignedLicense.TryParse("{\"payload\":\"\"}"));
    }

    [Fact]
    public void Verifier_RefusesPrivateKey()
    {
        var (priv, _, _) = Keys();
        var withPrivate = priv.ExportParameters(true); // contains D
        Assert.Throws<ArgumentException>(() => new LicenseVerifier(withPrivate, "k"));
    }

    [Fact]
    public void CanonicalJson_IsDeterministic()
    {
        var a = Payload("k").ToCanonicalJson();
        var b = Payload("k").ToCanonicalJson();
        Assert.Equal(a, b);
    }
}
