using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.ViewModel
{
    public class SignUpViewModel : ViewModel
    {
        private string _username;
        private string _password1;
        private string _password2;
        private string _email;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password1
        {
            get => _password1;
            set
            {
                _password1 = value;
                OnPropertyChanged(nameof(Password1));
            }
        }

        public string Password2
        {
            get => _password2;
            set
            {
                _password2 = value;
                OnPropertyChanged(nameof(Password2));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }
}
