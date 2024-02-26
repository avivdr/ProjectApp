using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.ViewModel
{
    public class StartupViewModel : ViewModel
    {
        readonly IPopupService popupService;
        public StartupViewModel(IPopupService _popupService)
        {
            popupService = _popupService;
                
            popupService.ShowPopup<StartupViewModel>();
        }
    }
}
