using ProjectApp.ViewModel;

namespace ProjectApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}

