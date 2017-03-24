using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfAsync.Annotations;

namespace WpfAsync
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _result;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Result
        {
            get { return _result; }
            set
            {
                if (value == _result) return;
                _result = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}