using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AwesomeTodo.Module.Calendar.ViewModels
{
    internal class CalendarViewModel : BindableBase 
    {
        public readonly int Columns = 7;
        public readonly int Rows = 6;

        public ObservableCollection<CalendarItem> CalendarItems { get; private set; }

        public CalendarViewModel()
        {
            InitCalendar();
        }

        private void InitCalendar()
        {
            CalendarItems = new ObservableCollection<CalendarItem>();

            foreach (var date in GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            {
                CalendarItems.Add(new CalendarItem { Date = date });
            }
        }

        private IEnumerable<DateTime> GetDaysInMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                .Select(day => new DateTime(year, month, day))
                .ToList();
        }

        public class CalendarItem : BindableBase
        {
            private string _displayDate;
            private DateTime? _date;

            public string DisplayDate => _date != null ? ((DateTime)_date).ToString("%d MMM") : null;

            public DateTime? Date
            {
                get => _date;
                set => SetProperty(ref _date, value);
            }
        }
    }
}
