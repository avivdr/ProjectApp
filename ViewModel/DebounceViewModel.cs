using ProjectApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.ViewModel
{
    public class DebounceViewModel : ViewModel
    {
        private string _entryText;
        private string _lblText;
        public string EntryText 
        {
            get => _entryText;
            set
            {
                _entryText = value;
                OnPropertyChanged(nameof(EntryText));
                Action a = () => LblText = _entryText;
                a.Debounce()();
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
    }
}
