using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class UploadPost : ContentPage
{
	readonly UploadPostViewModel vm;
	public UploadPost(UploadPostViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		this.vm = vm;
	}

    private void WorksScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
		vm.WorksScrolled(sender, e);
    }
}