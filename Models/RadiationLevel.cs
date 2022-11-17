using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2.Models
{
    public class RadiationLevel : INotifyPropertyChanged
    {
        private double _csLevel;
        public double CsLevel
        {
            get => _csLevel;
            set
            {
                SetField(ref _csLevel, value);
            }
        }

        private double _srLevel;
        public double SrLevel
        {
            get => _srLevel;
            set
            {
                SetField(ref _srLevel, value);
            }
        }

        public RadiationLevel(double csLevel = 0, double srLevel = 0)
        {
            CsLevel = csLevel;
            SrLevel = srLevel;
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
