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
using DebounceThrottle;
using ProjectApp.View;

namespace ProjectApp.ViewModel
{    
    public class UploadPostViewModel : ViewModel
    {
        const string SERVER_ERROR = "A server error occurred";
        const string FILE_PICK_ERROR = "An error occurred when picking file";
        const string INVALID = "Invalid fields";
        const string SHORT_QUERY = "Search query must be at least 4 characters";

        readonly Service service;

        private string _title;
        private string _content;
        private string _errorMessage;
        private bool _isErrorMessage;
        private string _query;
        private List<Composer> _composerResults;
        private Composer _composer;

        readonly DebounceDispatcher dispatcher;

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

                dispatcher.Debounce(() => Search(_query));
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
        public Composer Composer
        {
            get => _composer;
            set
            {
                _composer = value;
                OnPropertyChanged(nameof(Composer));
            }
        }
        public ICommand PostCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        public FileResult File { get; protected set; }
        public UploadPostViewModel(Service _service)
        {
            service = _service;
            IsErrorMessage = false;
            ErrorMessage = SERVER_ERROR;
            dispatcher = new DebounceDispatcher(100);

            PostCommand = new Command(async () =>
            {
                try
                {
                    User u = JsonSerializer.Deserialize<User>(await SecureStorage.GetAsync("CurrentUser"));
                    Post post = new()
                    {
                        Content = Content,
                        Title = Title,
                        Creator = u,
                        Composer = Composer,
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
        }

        private async void Search(string query)
        {
            if (query.Length < 4)
            {
                ComposerResults = new List<Composer>();
                IsErrorMessage = false;
                return;
            }

            var results = await service.SearchComposersByName(Query);
            if (results == null)
            {
                ErrorMessage = SERVER_ERROR;
                IsErrorMessage = true;
                ComposerResults = new List<Composer>();
                return;
            }

            IsErrorMessage = false;

            if (results.Count == 0)
            {
                ComposerResults = new()
                {
                    new() { CompleteName = "No result found :(" }
                };
                return;
            }

            ComposerResults = results;
        }
    }
}
