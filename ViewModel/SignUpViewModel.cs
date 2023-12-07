using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Model;
using ProjectApp.Services;

namespace ProjectApp.ViewModel
{
    public class SignUpViewModel : ViewModel
    {
        const string SERVER_ERROR = "A server error occurred";
        const string CONFLICT = "Username already exists";
        const string INVALID = "Invalid fields";
        private Service service;

        #region fields
        private string _username;
        private string _password1;
        private string _password2;
        private string _email;
        private bool _isErrorMessage;
        private string _errorMessage;
        #endregion

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
        public bool IsErrorMessage
        {
            get => _isErrorMessage;
            set
            {
                _isErrorMessage = value;
                OnPropertyChanged(nameof(IsErrorMessage));
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

        public ICommand SignUpCommand { get; protected set; }

        public SignUpViewModel(Service _service)
        {
            Username = "";
            Email = "";
            Password1 = "";
            Password2 = "";
            ErrorMessage = INVALID;
            IsErrorMessage = false;

            service = _service;

            SignUpCommand = new Command(async () =>
            {
                ErrorMessage = INVALID;
                if (!ValidateSignUp())
                {
                    IsErrorMessage = true;
                    return;
                }

                User user = new() { Username = Username, Email = Email, Password = Password1 };
                try
                {
                    HttpStatusCode statuscode = await service.Register(user);
                    switch (statuscode)
                    {
                        case HttpStatusCode.OK:
                            IsErrorMessage = false;
                            await SecureStorage.Default.SetAsync("CurrentUser", JsonSerializer.Serialize(user));
                            await Shell.Current.DisplayAlert("sign up success", "sign succeeded", "ok");
                            await Shell.Current.GoToAsync("MainPage");
                            break;

                        case HttpStatusCode.Conflict:
                            ErrorMessage = CONFLICT;
                            IsErrorMessage = true;
                            break;

                        default:
                            throw new Exception();
                    }
                }
                catch (Exception)
                {
                    ErrorMessage = SERVER_ERROR;
                    IsErrorMessage = true;
                }
            });
        }

        private bool ValidateSignUp()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password1) && !string.IsNullOrEmpty(Password2) && !string.IsNullOrEmpty(Email) 
                && Username.Length > 3 && Password1.Length > 3 && Password1 == Password2 && Email.Contains('@');
        }
    }
}

