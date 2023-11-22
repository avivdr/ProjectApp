using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectApp.ViewModel
{
    public class SignUpViewModel : ViewModel
    {
        private string _username;
        private string _password1;
        private string _password2;
        private string _email;
        private bool _isErrorMessage;
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

        public SignUpViewModel()
        {
            
        }

        private bool ValidateSignUp()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password1) && !string.IsNullOrEmpty(Password2) && !string.IsNullOrEmpty(Email) 
                && Username.Length > 3 && Password1.Length > 3 && Password1 == Password2 && Email.Contains('@');
        }
    }
}

