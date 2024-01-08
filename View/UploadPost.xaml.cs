using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class UploadPost : ContentPage
{
	//readonly UploadPostViewModel _vm;

    public UploadPost(UploadPostViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		//_vm = vm;
	}

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    _vm.CheckAccess();
    //}

    //  private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    //  {
    //_vm.WorksScrolled(sender, e);
    //  }
}