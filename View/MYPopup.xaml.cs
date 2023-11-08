using CommunityToolkit.Maui.Views;
using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class MYPopup : Popup
{
	public MYPopup(MYPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}