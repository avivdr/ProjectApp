using ProjectApp.Model;
using ProjectApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net;
using System.Collections.ObjectModel;

namespace ProjectApp.ViewModel
{
    [QueryProperty(nameof(Post), "Post")]
    public class ViewPostViewModel : ViewModel
    {
        const string UnknownError = "An error occurred uploading the comment";
        const string Empty = "Comment Content cannot be empty";
        const string Unauthorized = "Please login to upload a comment";

        readonly UserService userService;
        readonly Service service;

        private Post _post;
        private ObservableCollection<Comment> _comments;
        private string _content;
        private string _errorMessage;
        private bool _isError;

        public Post Post
        {
            get => _post;
            set
            {
                _post = value;
                OnPropertyChanged(nameof(Post));
            }
        }

        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                OnPropertyChanged(nameof(Comments));
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
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
        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }

        public ICommand UploadCommentCommand { get; set; }
        public ICommand BackBtnCommand { get; set; }

        public EventHandler LoadComments { get; private set; }

        public ViewPostViewModel(UserService _userService, Service _service)
        {
            userService = _userService;
            service = _service;

            IsError = false;
            ErrorMessage = UnknownError;

            LoadComments = new((s, e) => 
            { 
                Comments = new(Post.Comments);
            });

            BackBtnCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            UploadCommentCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(Content))
                {
                    IsError = true;
                    ErrorMessage = Empty;
                }

                IsError = false;

                Comment comment = new()
                {
                    Content = Content,
                    Creator = await userService.GetUser(),
                    Post = Post,
                    UploadDateTime = DateTime.Now
                };

                var response = await service.UploadComment(comment);

                switch (response)
                {
                    case StatusEnum.OK:
                        IsError = false;
                        Comments.Insert(0, comment);
                        OnPropertyChanged(nameof(Comments));
                        break;
                    case StatusEnum.Unauthorized:
                        IsError = true;
                        ErrorMessage = Unauthorized; 
                        break;
                    default:
                        IsError = true;
                        ErrorMessage = UnknownError;
                        break;
                }
                
               Content = "";
            });
        }
    }
}
