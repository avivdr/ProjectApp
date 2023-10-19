using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectApp.ViewModel
{
    public class LoginViewModel : ViewModel
    {
        private string _username;
        private string _password;
        public string UserName 
        {
            get { return _username; }
            set 
            { _username= value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand BtnCommand { get; protected set; }

        public LoginViewModel()
        {
            UserName = "";
            Password = "";

            BtnCommand = new Command(async () =>
            {
                throw new NotImplementedException();
            });
        }
    }
}
