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
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

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
        private ObservableCollection<Composer> _composerResults;
        private ObservableCollection<Work> _workResults;
        private dynamic _selection;

        readonly DebounceDispatcher searchDebounce;
        readonly ThrottleDispatcher loadDebounce;

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
        public dynamic Selection
        {
            get => _selection;
            set
            {
                _selection = value;
                OnPropertyChanged(nameof(Selection));
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

                searchDebounce.Debounce(() => Search(_query));
            }
        }
        public ObservableCollection<Composer> ComposerResults { get; set; }
        public ObservableCollection<Work> WorkResults { get; set; }
        public ICommand PostCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        public FileResult File { get; protected set; }
        public UploadPostViewModel(Service _service)
        {
            service = _service;
            IsErrorMessage = false;
            ErrorMessage = SERVER_ERROR;
            searchDebounce = new(300);
            loadDebounce = new(700);

            ComposerResults = new();
            WorkResults = new();

            //post command
            PostCommand = new Command(async () =>
            {
                try
                {
                    User user = await service.GetCurrentUser();
                    Post post = new()
                    {
                        Content = Content,
                        Title = Title,
                        Creator = user,
                    };

                    if (Selection is Work)
                    {
                        post.Work = Selection;
                        post.Composer = null;
                    }
                    if (Selection is Composer)
                    {
                        post.Composer = Selection;
                        post.Work = null;
                    }

                    HttpStatusCode httpStatusCode = await service.UploadPost(post, File);
                    switch (httpStatusCode)
                    {
                        case HttpStatusCode.OK:
                            await Shell.Current.DisplayAlert("Post uploaded", "post uploaded successfully", "ok");
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

            //pick file
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

        public void WorksScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            int lastIndex = e.LastVisibleItemIndex;

            if (WorkResults.Count - lastIndex > 9)
                return;

            loadDebounce.Throttle(async () =>
            {
                //load more works
                OmniSearchDTO results = await service.NextOmniSearch();
                if (results == null) return;

                ComposerResults.AddRange(results.Composers);
                WorkResults.AddRange(results.Works);

                (sender as CollectionView).ScrollTo(lastIndex);
            });
        }

        private async void Search(string query)
        {
            if (query.Length < 4)
            {
                ComposerResults.Empty();
                WorkResults.Empty();
                IsErrorMessage = false;
                return;
            }

            OmniSearchDTO results = await service.OmniSearch(Query);
            if (results == null)
            {
                ErrorMessage = SERVER_ERROR;
                IsErrorMessage = true;
                ComposerResults.Empty();
                WorkResults.Empty();
                return;
            }

            IsErrorMessage = false;

            if (results.Composers.Count == 0)
            {
                ComposerResults.Empty();
            }
            else ComposerResults.SetFromList(results.Composers);

            if (results.Works.Count == 0)
            {
                WorkResults.Empty();
            }
            else WorkResults.SetFromList(results.Works);
        }
    }
}
