using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class ViewPost : ContentPage
{
	public ViewPost(ViewPostViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
		Loaded += vm.LoadComments;
	}
}