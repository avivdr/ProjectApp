﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using ProjectApp.Model;
using System.Text.Json;

namespace ProjectApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private readonly IPopupService popupService;
        public ICommand BtnCommand { get; set; }
        public ICommand Btn2Command { get; set; }
        public ICommand Btn3Command { get; set; }

        private string _password;
        private string _username;
        private string _email;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
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

        public MainPageViewModel(IPopupService _popupService)
        {
            popupService = _popupService;
            BtnCommand = new Command(popupService.ShowPopup<LoginViewModel>);
            Btn2Command = new Command(async () =>
            {
                User u = JsonSerializer.Deserialize<User>(await SecureStorage.Default.GetAsync("CurrentUser"));
                Username = u.Username;
                Password = u.Password;
                Email = u.Email;
            });
            Btn3Command = new Command(async () => await Shell.Current.GoToAsync("//Debounce"));
        }
    }
}
