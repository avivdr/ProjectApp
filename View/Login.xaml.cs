using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class Login : ContentPage
{
	public Login(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}