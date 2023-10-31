using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prio.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public int DateDifferenceInDays { get; set; }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged(nameof(DueDate));
                }
            }
        }

        private void CalculateDateDifference()
        {
            DateTime selectedDate = DueDate;
            DateTime currentDate = DateTime.Today;
            TimeSpan difference = selectedDate - currentDate;
            DateDifferenceInDays = difference.Days + 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
