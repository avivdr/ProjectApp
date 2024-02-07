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
using Syncfusion.Maui.ListView;
using CommunityToolkit.Maui.Core.Extensions;

namespace ProjectApp.ViewModel
{
    public class UploadPostViewModel : ViewModel
    {
        const string ServerError = "A server error occurred";
        const string FilePickError = "An error occurred when picking file";
        const string Empty = "Title cannot be empty";
        const string TagMessage = "Tag Work or Composer";
        const string Unauthorized = "Please login to upload a post";

        readonly PickOptions[] filePickOptions = { null,
            new() { PickerTitle = "Image", FileTypes = FilePickerFileType.Images },
            new() { PickerTitle = "Video", FileTypes = FilePickerFileType.Videos },
            new() { PickerTitle = "Audio", FileTypes = new(
                new Dictionary<DevicePlatform, IEnumerable<string>>()
                {
                    { DevicePlatform.WinUI, new[] { ".wav", ".mp3", ".m4a" } },
                    { DevicePlatform.Android, new[] { "audio/mpeg", "audio/wav", "audio/mp4"} }
                })}};

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
        private int _selectedTab;
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
                OnPropertyChanged(nameof(FilePickBtnText));
                OnPropertyChanged(nameof(IsFileSelected));
            }
        }
        public TaggableItem Selection
        {
            get => _selection;
            set
            {
                _selection = value;
                OnPropertyChanged(nameof(Selection));
            }
        }
        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }
        public string FilePickBtnText
        {
            get => FileResult?.FileName ?? "Pick File";
        }
        public bool IsFileSelected
        {
            get => FileResult != null;
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

        public ICommand UploadPostCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        public ICommand LoadMoreWorks { get; protected set; }
        public ICommand OpenPopup { get; protected set; }
        public ICommand ClosePopup { get; protected set; }
        public ICommand ClearSelection { get; protected set; }
        public UploadPostViewModel(Service _service)
        {
            service = _service;

            IsErrorMessage = false;
            ErrorMessage = ServerError;
            searchDebounce = new(300);
            ComposerResults = null;
            WorkResults = null;

            Selection = new TaggableItem(TagMessage);

            OpenPopup = new Command(() => IsPopupOpen = true);
            ClosePopup = new Command(() => IsPopupOpen = false);
            ClearSelection = new Command(() =>
            {
                Selection = new TaggableItem(TagMessage);
                IsPopupOpen = false;
            });

            //post command
            UploadPostCommand = new Command(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(Title))
                    {
                        ErrorMessage = Empty;
                        IsErrorMessage = true;
                        return;
                    }
                    if (FileResult == null && SelectedTab != 0 &&
                        !await Shell.Current.DisplayAlert("Empty file", "Do you wish to upload a post without a selected file?", "Yes", "No, cancel"))
                            return;

                    User user = await service.GetCurrentUser();
                    Post post = new()
                    {
                        Content = Content,
                        Title = Title,
                        Creator = user,
                        FileExtention = Path.GetExtension(FileResult?.FullPath)
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

                        case HttpStatusCode.Unauthorized:
                            ErrorMessage = Unauthorized;
                            IsErrorMessage = true;
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
                if (SelectedTab == 0) return;

                try
                {
                    FileResult = await FilePicker.Default.PickAsync(filePickOptions[SelectedTab]);
                }
                catch (Exception)
                {
                    FileResult = null;
                    ErrorMessage = FilePickError;
                    IsErrorMessage = true;
                }
            });

            //load more works
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

        public void FilterWorksBySelectedComposer()
        {
            if (WorkResults == null || Selection == null) return;

            if (Selection is Composer selectedComposer)
            {
                WorkResults = WorkResults.Where(x => x.Composer.CompleteName == selectedComposer.CompleteName).ToObservableCollection();
            }
        }


        public async void CheckAccess()
        {
            return;

            if (await service.GetCurrentUser() == null)
            {
                await Shell.Current.GoToAsync("//MainPage");
                await Shell.Current.DisplayAlert("Access Denied", "You must be logged in to enter this page", "Return to main page");
            }
        }
    }
}
