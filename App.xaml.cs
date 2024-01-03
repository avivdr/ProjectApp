namespace ProjectApp;

public partial class App : Application
{
	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzAxMjU1MUAzMjM0MmUzMDJlMzBZZmtNQzhQa0o1NE5NY2IzZ3IyWFM5L290S2tVamJoNjM0WkQ3OW1qOG13PQ==");

        InitializeComponent();

		MainPage = new AppShell();
	}
}
