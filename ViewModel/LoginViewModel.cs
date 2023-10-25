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

        public LoginViewModel()
        {
            Username = "";
            Password = "";
            IsLoginError = true;
            ErrorMessage = "Incorrect username or password";

            LoginCommand = new Command(async () =>
            {
                if (!validateUser(Username, Password)) 
                    return;

                var service = new Service();
                try
                {
                    User user = await service.Login(Username, Password);
                    if (user is null)
                    {
                        IsLoginError = true;
                    }
                    else
                    {
                        IsLoginError = false;

                        await SecureStorage.SetAsync("CurrentUser", JsonSerializer.Serialize(user));
                        await Shell.Current.DisplayAlert("logged in message", "Logged in!", "OK");
                    }
                }
                catch (Exception)
                {
                    ErrorMessage = "A server error occurred";
                    IsLoginError = true;
                }
               
            });
        }

        private bool validateUser(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && username.Length > 3 && password.Length > 3;
        }
    }
}