using ProjectApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebounceThrottle;

namespace ProjectApp.ViewModel
{
    public class DebounceViewModel : ViewModel
    {
        readonly DebounceDispatcher _dispatcher;

        private string _entryText;
        private string _lblText;
        public string EntryText 
        {
            get => _entryText;
            set
            {
                _entryText = value;
                OnPropertyChanged(nameof(EntryText));
                _dispatcher.Debounce(() => Change(value));
            }
        }
        public string LblText
        {
            get => _lblText;
            set
            {
                _lblText = value;
                OnPropertyChanged(nameof(LblText));
            }
        }

        public DebounceViewModel()
        {
            _dispatcher = new DebounceDispatcher(1000);
        }

        private async void Change(string text)
        {
            await Task.Delay(1500);
            LblText = text;
        }
    }
}
