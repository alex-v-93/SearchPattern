using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using SearchPattern;
using SearchPatternDemo.Annotations;
using System.Windows.Input;

namespace SearchPatternDemo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        IEnumerable<string> _resultSearch = new List<string>();
        private RelayCommand _addCommand;

        private volatile bool _isProccess;
        string _pattern = "";
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand _searchCommand;
        Cursor _cursor = Cursors.Arrow;
        #endregion
        #region Properties
        public Cursor Cursor
        {
            get { return _cursor; }
            set
            {
                if (value == _cursor)
                    _cursor = value;
                _cursor = value;
                OnPropertyChanged();
            }
        }
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                if (value == _pattern)
                    return;
                _pattern = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> Result
        {
            get { return _resultSearch; }
            set
            {
                if (value == _resultSearch)
                    return;
                _resultSearch = value;
                OnPropertyChanged();
            }
        }

        private IWildcardSearcher Searcher { get; }


        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                       (_addCommand = new RelayCommand(async obj =>
                       {
                           _isProccess = true;

                           var ofd = new OpenFileDialog {Filter = "Текстовые файлы (*.txt)|*.txt"};

                           if (ofd.ShowDialog() == true)
                           {
                               Cursor = Cursors.Wait;
                               await AddWordsAsync(ofd.FileName);
                           }
                           Cursor = Cursors.Arrow;
                           _isProccess = false;
                       }, obj => !_isProccess));
            }
        }

        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                       (_searchCommand = new RelayCommand(async obj =>
                       {
                           if (obj == null)
                               return;
                           var pattern = (string)obj;
                           
                           if (pattern.Length == 0)
                               return;
                           _isProccess = true;


                           Result = await Task.Run(() => Searcher.SearchWords(pattern));


                           _isProccess = false;
                       }, obj => !_isProccess));
            }
        }

        #endregion
        public MainViewModel(IWildcardSearcher searcher)
        {
            Searcher = searcher;
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private Task AddWordsAsync(string fileName)
        {
            return Task.Run(() =>
            {
                using (var file = SearcherFactory.GetDictionaryReader(fileName))
                {
                    string word;
                    while ((word = file.GetWord()).Length > 0)
                    {
                        Searcher.AddWord(word);
                    }
                }
            });
        }



    }
}
