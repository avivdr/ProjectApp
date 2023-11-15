using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class SignUp : ContentPage
{
	public SignUp(SignUpViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}