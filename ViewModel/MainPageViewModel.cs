using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Services;

namespace ProjectApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        public ICommand BtnCommand { get; set; }
        private string _lblText;
        public string LblText
        {
            get => _lblText;
            set
            {
                _lblText = value;
                OnPropertyChanged(nameof(LblText));
            }
        }
            
        public MainPageViewModel()
        {
            BtnCommand = new Command(async () =>
            {
                Service service = new Service();
                LblText = await service.GetHello();            
            });
        }
    }
}
