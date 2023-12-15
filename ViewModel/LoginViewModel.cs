using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Services;
using ProjectApp.Model;
using System.Text.Json;
using ProjectApp.View;

namespace ProjectApp.ViewModel
{
    public class LoginViewModel : ViewModel
    {
        const string INCORRECT = "Incorrect username or password";
        const string SERVER_ERROR = "A server error occurred";

        private Service service;

        private string _username;
        private string _password;
        private bool _isLoginError;
        private string _errorMessage;
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; protected set; }
        public ICommand SignUpBtnCommand { get; protected set; }    

        public LoginViewModel(Service _service)
        {
            Username = "";
            Password = "";
            IsLoginError = false;
            ErrorMessage = INCORRECT;
            service = _service;

            SignUpBtnCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//SignUp");
                await Login.CloseInstanceAsync();
            });

            LoginCommand = new Command(async () =>
            {
                IsLoginError = false;
                ErrorMessage = INCORRECT;

                if (!ValidateUser())
                {
                    IsLoginError = true;
                    return;
                }

                try
                {
                    User user = await service.Login(Username, Password);
                    if (user == null)
                    {
                        IsLoginError = true;
                    }
                    else
                    {
                        IsLoginError = false;

                        await Shell.Current.DisplayAlert("logged in message", "Logged in!", "OK");
                        await Shell.Current.GoToAsync("//UploadPost");
                        await Login.CloseInstanceAsync();
                    }
                }
                catch (Exception)
                {
                    ErrorMessage = SERVER_ERROR;
                    IsLoginError = true;
                }
            });
        }

        private bool ValidateUser()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && Username.Length > 3 && Password.Length > 3;
        }
    }
}