using CommunityToolkit.Maui.Views;
using ProjectApp.ViewModel;

namespace ProjectApp.View;

public partial class MYPopup : Popup
{
	public static MYPopup Instance { get; private set; }
	public MYPopup(MYPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		Instance = this;
	}

	public static void CloseCurrent()
	{
		Instance.Close();
		Instance = null;
	}
}