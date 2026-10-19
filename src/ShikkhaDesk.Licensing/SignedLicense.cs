using System.Text.Json;

namespace ShikkhaDesk.Licensing;

/// <summary>
/// The exported license file: the canonical payload string (exactly what was
/// signed), its Base64Url signature, and the signing-key id. Verification is done
/// over the exact payload bytes to avoid any canonicalization mismatch on parse.
/// </summary>
public sealed class SignedLicense
{
    public string Payload { get; }
    public string Signature { get; }
    public string SigningKeyId { get; }

    public SignedLicense(string payload, string signature, string signingKeyId)
    {
        Payload = payload;
        Signature = signature;
        SigningKeyId = signingKeyId;
    }

    private sealed class FileModel
    {
        public string payload { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;
        public string signingKeyId { get; set; } = string.Empty;
    }

    public string ToJson() => JsonSerializer.Serialize(new FileModel
    {
        payload = Payload,
        signature = Signature,
        signingKeyId = SigningKeyId
    });

    /// <summary>Parse a license file. Returns null if malformed (never throws on bad input).</summary>
    public static SignedLicense? TryParse(string json)
    {
        try
        {
            var model = JsonSerializer.Deserialize<FileModel>(json);
            if (model is null || string.IsNullOrWhiteSpace(model.payload)
                || string.IsNullOrWhiteSpace(model.signature))
                return null;
            return new SignedLicense(model.payload, model.signature, model.signingKeyId ?? string.Empty);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
