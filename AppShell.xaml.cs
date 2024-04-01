using ProjectApp.View;

namespace ProjectApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("ViewPost", typeof(ViewPost));
	}
}
