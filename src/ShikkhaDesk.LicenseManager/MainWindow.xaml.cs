using System.Security.Cryptography;
using System.Windows;
using ShikkhaDesk.Licensing;

namespace ShikkhaDesk.LicenseManager;

public partial class MainWindow : Window
{
    // DEVELOPMENT ONLY: a clearly-labeled TEST key. The production private key is
    // generated and held in a separate, access-restricted signing environment and
    // is NEVER embedded in any distributed application (LICENSE-DESIGN.md / DEC-3).
    private string _testKeyId = "test-key-2026";

    public MainWindow()
    {
        InitializeComponent();
        InstitutionUidBox.Text = Guid.NewGuid().ToString("D");
    }

    private void OnSignClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!Guid.TryParse(InstitutionUidBox.Text.Trim(), out var uid))
            {
                OutputBox.Text = "Invalid institution UID."; return;
            }
            if (!int.TryParse(MaxDevicesBox.Text.Trim(), out var maxDevices))
            {
                OutputBox.Text = "Invalid max devices."; return;
            }

            _testKeyId = "test-key-2026";
            var (priv, _, keyId) = LicenseSigner.CreateTestKey(_testKeyId);
            using var signer = new LicenseSigner(priv, keyId);

            var payload = new LicensePayload
            {
                FormatId = LicensePayload.Format,
                LicenseId = "LIC-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
                Product = LicensePayload.ProductId,
                InstitutionUid = uid,
                InstitutionName = InstitutionNameBox.Text.Trim(),
                LicenseType = "Perpetual",
                MaxDevices = maxDevices,
                VersionEntitlement = VersionEntitlementBox.Text.Trim(),
                IssuedAtUtc = DateTime.UtcNow,
                SigningKeyId = _testKeyId
            };

            var signed = signer.Sign(payload);
            OutputBox.Text = "License created and signed (TEST KEY). Export file:\r\n\r\n" + signed.ToJson();
        }
        catch (Exception ex)
        {
            OutputBox.Text = "Error: " + ex.Message;
        }
    }
}
