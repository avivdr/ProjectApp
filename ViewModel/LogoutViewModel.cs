using ProjectApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.ViewModel
{
    public class LogoutViewModel : ViewModel
    {
        readonly UserService userService;
        public EventHandler Logout { get; private set; }

        public LogoutViewModel(UserService _userService)
        {
            userService = _userService;

            Logout = new(async (s, e) =>
            {
                await userService.SetUser(null);
                await Task.Delay(2500);
                await Shell.Current.GoToAsync("//MainPage");
            });
        }
    }
}
