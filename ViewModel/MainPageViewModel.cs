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
using System.Collections.ObjectModel;

namespace ProjectApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private readonly IPopupService popupService;
        readonly Service service;
        readonly UserService userService;

        private ObservableCollection<Post> _posts;
        private Post _selectedPost;
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        public Post SelectedPost
        {
            get => _selectedPost;
            set
            {
                _selectedPost = value;
                OnPropertyChanged(nameof(SelectedPost));
            }
        }

        public ICommand BtnCommand { get; set; }
        public ICommand PostClickedCommand { get; set; }
        public ICommand DeletePostCommand { get; set; }


        public EventHandler Login { get; set; }

        public MainPageViewModel(IPopupService _popupService, Service _service, UserService _userService)
        {
            popupService = _popupService;
            service = _service;
            userService = _userService;

            BtnCommand = new Command(popupService.ShowPopup<LoginViewModel>);

            DeletePostCommand = new Command(async p =>
            {
                Post post = (Post)p;
                bool result = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this post?", "Yes", "No");
                if (!result) 
                    return;

                var response = await service.DeletePost(post.Id);
                if (response == StatusEnum.OK)
                    Posts.Remove(post);
                else
                    await Shell.Current.DisplayAlert("Error", "An error has occurred", "Ok");
            });

            Login = new(async (s,e) => 
            {
                Posts = new();
                if (!userService.IsLoggedIn)
                    await popupService.ShowPopupAsync<LoginViewModel>();

                User = await userService.GetUser();
                Posts = new(await service.GetAllPosts());
            });

            PostClickedCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("ViewPost", new Dictionary<string, object>()
                {
                    { "Post", SelectedPost}
                });
            });
        }
    }
}
