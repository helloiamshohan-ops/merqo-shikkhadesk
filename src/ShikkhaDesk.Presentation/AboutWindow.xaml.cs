using System.Windows;

namespace ShikkhaDesk.Presentation;

/// <summary>
/// About screen (PART 1.2). Shows the required MERQO ShikkhaDesk identity and
/// contact. No invented address/phone/website/registration. Values are populated
/// from the running assembly/settings at runtime.
/// </summary>
public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        VersionText.Text = $"অ্যাপ সংস্করণ: {v?.ToString() ?? "1.0.0"}";
        BuildText.Text = $"বিল্ড/রিলিজ: {v?.ToString() ?? "1.0.0"}";
    }

    private void OnOkClick(object sender, RoutedEventArgs e) => Close();
}
