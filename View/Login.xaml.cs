using CommunityToolkit.Maui.Views;
using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class Login : Popup
{
	public static Login Instance { get; private set; }
	public Login(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		Instance = this;
	}

	public static async Task CloseInstanceAsync()
	{
		if (Instance == null) return;

		await Instance.CloseAsync();
		Instance = null;
	}
}