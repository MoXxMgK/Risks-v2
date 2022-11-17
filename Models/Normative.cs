using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2.Models
{
    public class Normative : INotifyPropertyChanged
    {
        private bool _csNormative;
        public bool CsNormative
        {
            get => _csNormative;
            set
            {
                SetField(ref _csNormative, value);
            }
        }

        private bool _srNormative;
        public bool SrNormative
        {
            get => _srNormative;
            set
            {
                SetField(ref _srNormative, value);
            }
        }

        private bool _csEnabled = true;
        public bool CsEnabled
        {
            get => _csEnabled;
            set
            {
                SetField(ref _csEnabled, value);
            }
        }

        private bool _srEnabled = true;
        public bool SrEnabled
        {
            get => _srEnabled;
            set
            {
                SetField(ref _srEnabled, value);
            }
        }

        public Normative(bool csNormative = false, bool srNormative = false)
        {
            CsNormative = csNormative;
            SrNormative = srNormative;
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
