using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class Debounce : ContentPage
{
	public Debounce(DebounceViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}