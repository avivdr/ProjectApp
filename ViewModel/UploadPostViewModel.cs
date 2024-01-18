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
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.ListView;

namespace ProjectApp.ViewModel
{
    public class UploadPostViewModel : ViewModel
    {
        const string ServerError = "A server error occurred";
        const string FilePickError = "An error occurred when picking file";
        const string Invalid = "Invalid fields";
        const string ShortQuery = "Search query must be at least 4 characters";
        const string TagMessage = "Tag Work or Composer";

        readonly Service service;
        readonly DebounceDispatcher searchDebounce;
        private bool MoreWorksToLoad = true;

        #region fields
        private string _title;
        private string _content;
        private string _errorMessage;
        private bool _isErrorMessage;
        private bool _isWorksLoading;
        private bool _isPopupOpen;
        private string _query;
        private ObservableCollection<Composer> _composerResults;
        private ObservableCollection<Work> _workResults;
        private dynamic _selection;
        private FileResult _fileResult;
        #endregion

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
        public bool IsWorksLoading
        {
            get => _isWorksLoading;
            set
            {
                _isWorksLoading = value;
                OnPropertyChanged(nameof(IsWorksLoading));
            }
        }
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }
        public ObservableCollection<Composer> ComposerResults
        {
            get => _composerResults;
            set
            {
                _composerResults = value;
                OnPropertyChanged(nameof(ComposerResults));
            }
        }
        public ObservableCollection<Work> WorkResults
        {
            get => _workResults;
            set
            {
                _workResults = value;
                OnPropertyChanged(nameof(WorkResults));
            }
        }
        public FileResult FileResult
        {
            get => _fileResult;
            set
            {
                _fileResult = value;
                OnPropertyChanged(nameof(FileResult));
            }
        }
        public object Selection
        {
            get => _selection;
            set
            {
                if (value != null)
                {
                    _selection = value;
                    OnPropertyChanged(nameof(Selection));
                    OnPropertyChanged(nameof(TagText));
                    OnPropertyChanged(nameof(TagImageSource));
                    OnPropertyChanged(nameof(IsTagImageVisible));
                }
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
        
        public string TagText
        {
            get
            {
                if (Selection != null)
                {
                    if (Selection is Composer composer)
                        return composer.CompleteName;
                    if (Selection is Work work)
                        return work.TitleWithComposersName;
                }
                return TagMessage;
            }
        }
        public string TagImageSource
        {
            get
            {
                if (Selection != null && Selection is Composer composer)
                    return composer.Portrait;

                return "";
            }
        }
        public bool IsTagImageVisible
        {
            get => !string.IsNullOrEmpty(TagImageSource);
        }
        public ICommand UploadPostCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        public ICommand LoadMoreWorks { get; protected set; }
        public ICommand OpenPopup { get; protected set; }
        public ICommand ClosePopup { get; protected set; }
        public UploadPostViewModel(Service _service)
        {
            service = _service;

            IsErrorMessage = false;
            ErrorMessage = ServerError;
            searchDebounce = new(300);
            ComposerResults = null;
            WorkResults = null;

            Selection = new Composer() { CompleteName = "Tag Work or Composer" };

            OpenPopup = new Command(() => IsPopupOpen = true);
            ClosePopup = new Command(() => IsPopupOpen = false);

            //post command
            UploadPostCommand = new Command(async () =>
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

                    if (Selection is Work work)
                    {
                        post.Work = work;
                        post.Composer = null;
                    }
                    if (Selection is Composer composer)
                    {
                        post.Composer = composer;
                        post.Work = null;
                    }

                    HttpStatusCode httpStatusCode = await service.UploadPost(post, FileResult);
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
                    ErrorMessage = ServerError;
                    IsErrorMessage = true;
                }
            });

            //pick file
            PickFileCommand = new Command(async () =>
            {
                try
                {
                    FileResult = await FilePicker.Default.PickAsync();
                }
                catch (Exception)
                {
                    FileResult = null;
                    ErrorMessage = FilePickError;
                    IsErrorMessage = true;
                }
            });

            LoadMoreWorks = new Command(async () =>
            {
                IsWorksLoading = true;

                OmniSearchDTO results = await service.NextOmniSearch();
                if (results != null)
                {
                    ComposerResults.AddRange(results.Composers);
                    WorkResults.AddRange(results.Works);
                }
                else MoreWorksToLoad = false;

                IsWorksLoading = false;

            }, () => MoreWorksToLoad);
        }

        private async void Search(string query)
        {
            if (query.Length < 4)
            {
                ComposerResults = null;
                WorkResults = null;
                IsErrorMessage = false;
                return;
            }

            MoreWorksToLoad = true;
            OmniSearchDTO results = await service.OmniSearch(Query);
            if (results == null)
            {
                ErrorMessage = ServerError;
                IsErrorMessage = true;
                ComposerResults = null;
                WorkResults = null;
                return;
            }

            IsErrorMessage = false;

            ComposerResults = (results.Composers.Count == 0) ? null : new(results.Composers);
            WorkResults = (results.Works.Count == 0) ? null : new(results.Works);
        }

        //public async void CheckAccess()
        //{
        //    if (await service.GetCurrentUser() == null)
        //    {
        //        await Shell.Current.DisplayAlert("Access Denied", "You must be logged in to enter this page", "Return to main page");
        //        await Shell.Current.GoToAsync("//MainPage");
        //        return;
        //    }
        //}
    }
}
