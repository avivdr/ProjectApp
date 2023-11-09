using CommunityToolkit.Maui.Core.Views;
using ProjectApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectApp.ViewModel
{
    public class MYPopupViewModel : ViewModel
    {
        public ICommand BtnCommand { get; private set; }
        public MYPopupViewModel()
        {
            BtnCommand = new Command(MYPopup.CloseCurrent);
        }
    }
}
