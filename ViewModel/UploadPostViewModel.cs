using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectApp.Services;
using ProjectApp.Model;
using System.Text.Json;
using System.Net;

namespace ProjectApp.ViewModel
{    
    public class UploadPostViewModel : ViewModel
    {
        const string SERVER_ERROR = "A server error occurred";
        const string FILE_PICK_ERROR = "An error occurred when picking file";
        const string INVALID = "Invalid fields";

        private Service service;

        private string _title;
        private string _content;
        private string _errorMessage;
        private bool _isErrorMessage;
        private string _query;
        private List<Composer> _composerResults;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
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
        public bool IsErrorMessage
        {
            get => _isErrorMessage;
            set
            {
                _isErrorMessage = value;
                OnPropertyChanged(nameof(IsErrorMessage));
            }
        }
        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                OnPropertyChanged(nameof(Query));
            }
        }
        public List<Composer> ComposerResults
        {
            get => _composerResults;
            set
            {
                _composerResults = value;
                OnPropertyChanged(nameof(ComposerResults));
            }
        }
        public ICommand PostCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        public ICommand SearchCommand { get; protected set; }
        public FileResult File { get; protected set; }
        public UploadPostViewModel(Service _service)
        {
            service = _service;
            IsErrorMessage = false;
            ErrorMessage = SERVER_ERROR;

            PostCommand = new Command(async () =>
            {
                try
                {
                    User u = JsonSerializer.Deserialize<User>(await SecureStorage.GetAsync("CurrentUser"));
                    Post post = new()
                    {
                        Content = Content,
                        Title = Title,
                        Creator = u
                    };
                    HttpStatusCode httpStatusCode = await service.UploadPost(post, File);
                    switch (httpStatusCode)
                    {
                        case HttpStatusCode.OK:
                            await Shell.Current.DisplayAlert("Post uploaded", "post uploaded successfully", "ok");
                            break;
                        case HttpStatusCode.BadRequest:
                            ErrorMessage = INVALID;
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

            PickFileCommand = new Command(async () =>
            {
                try
                {
                    File = await FilePicker.Default.PickAsync();
                }
                catch (Exception)
                {
                    File = null;
                    ErrorMessage = FILE_PICK_ERROR;
                    IsErrorMessage = true;
                }
            });

            SearchCommand = new Command(async () =>
            {

            });

        }
    }
}
