using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Risks_v2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected bool _busy = false;

        public bool Busy
        {
            get => _busy;
            set => SetField(ref _busy, value);
        }

        protected string _title = "";

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        #region Property Changed
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
