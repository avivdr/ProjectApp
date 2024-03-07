using System;
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
        readonly Service service;

        private List<Post> _posts;

        public List<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        public ICommand BtnCommand { get; set; }

        public EventHandler ShowPopupE { get; set; }

        public MainPageViewModel(IPopupService _popupService, Service _service)
        {
            popupService = _popupService;
            service = _service;

            BtnCommand = new Command(popupService.ShowPopup<LoginViewModel>);

            ShowPopupE = new EventHandler(async (s,e) => 
            {
                await popupService.ShowPopupAsync<LoginViewModel>();
                Posts = await service.GetAllPosts();
            });
        }
    }
}
