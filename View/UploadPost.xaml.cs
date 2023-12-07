using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class UploadPost : ContentPage
{
	public UploadPost(UploadPostViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}