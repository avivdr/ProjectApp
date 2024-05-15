using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class Logout : ContentPage
{
	public Logout(LogoutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		Appearing += vm.Logout;
	}
}