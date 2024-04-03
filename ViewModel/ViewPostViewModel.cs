using ProjectApp.Model;
using ProjectApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net;
using System.ComponentModel.Design;

namespace ProjectApp.ViewModel
{
    [QueryProperty(nameof(Post), "Post")]
    public class ViewPostViewModel : ViewModel
    {
        readonly UserService userService;
        readonly Service service;

        private Post _post;
        private string _content;

        public Post Post
        {
            get => _post;
            set
            {
                _post = value;
                OnPropertyChanged(nameof(Post));
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

        public ICommand UploadCommentCommand { get; set; }

        public ViewPostViewModel(UserService _userService, Service _service)
        {
            userService = _userService;
            service = _service;

            UploadCommentCommand = new Command(async () =>
            {
                Comment comment = new()
                {
                    Content = Content,
                    Creator = await userService.GetUser(),
                    Post = Post,
                    UploadDateTime = DateTime.Now
                };
                comment.CreatorId = comment.Creator.Id;
                comment.PostId = comment.Post.Id;

                var response = await service.UploadComment(comment);

                switch (response)
                {
                    case HttpStatusCode.OK:
                        Post.Comments.Insert(0, comment);
                        break;
                }
                
            });
        }
    }
}
