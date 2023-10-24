using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Services;

namespace ProjectApp.ViewModel
{
    public class LoginViewModel : ViewModel
    {
        private string _username;
        private string _password;
        private bool _isLoginError;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public bool IsLoginError
        {
            get => _isLoginError;
            set
            {
                _isLoginError = value;
                OnPropertyChanged(nameof(IsLoginError));
            }
        }

        public ICommand BtnCommand { get; protected set; }

        public LoginViewModel()
        {
            Username = "";
            Password = "";
            IsLoginError = false;

            BtnCommand = new Command(async () =>
            {
                var service = new Service();
                bool loginSucceeded = await service.Login(Username, Password);
                if (loginSucceeded)
                {
                    IsLoginError = false;
                }
                else
                {
                    _isLoginError = true;
                }
            });
        }
    }
}