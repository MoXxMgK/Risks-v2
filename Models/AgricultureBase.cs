using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2.Models
{
    public abstract class AgricultureBase : INotifyPropertyChanged
    {
        public int Id;
        public string Name { get; set; }

        public double FieldArea = 0;

        public double Cs = 0;

        public double CsActivity = -999;

        public double Sr = 0;

        public double SrActivity = -999;

        private bool _checked = false;

        public bool Checked
        {
            get => _checked;
            set
            {
                SetField(ref _checked, value);
                OnCheckedChanged?.Invoke(value);
            }
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set
            {
                value = Math.Max(value, 0);
                SetField(ref _quantity, value);
            }
        }

        public event Action<bool>? OnCheckedChanged;

        public AgricultureBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return this.Name;
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
