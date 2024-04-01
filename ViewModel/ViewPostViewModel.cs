using ProjectApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.ViewModel
{
    [QueryProperty(nameof(Post), "Post")]
    public class ViewPostViewModel : ViewModel
    {
        private Post _post;
        public Post Post
        {
            get => _post;
            set
            {
                _post = value;
                OnPropertyChanged(nameof(Post));
            }
        }
    }
}
