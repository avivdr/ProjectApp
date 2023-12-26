using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class UploadPost : ContentPage
{
	readonly UploadPostViewModel _vm;

    public UploadPost(UploadPostViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
	}

    private async void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
		await _vm.WorksScrolled(sender, e);
    }
}