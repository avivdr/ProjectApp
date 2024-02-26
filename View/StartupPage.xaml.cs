using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class StartupPage : ContentPage
{
	public StartupPage(StartupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}