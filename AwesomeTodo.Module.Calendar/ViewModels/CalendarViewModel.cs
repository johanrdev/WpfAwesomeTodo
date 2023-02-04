using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AwesomeTodo.Module.Calendar.ViewModels
{
    internal class CalendarViewModel : BindableBase 
    {
        private int _currentYear = DateTime.Now.Year;
        private int _currentMonth = DateTime.Now.Month - 1;

        public readonly int ColumnCount = 7;
        public readonly int RowCount = 6;

        public ObservableCollection<CalendarItem> CalendarItems { get; private set; }

        public CalendarViewModel()
        {
            InitCalendar();
        }

        private void InitCalendar()
        {
            CalendarItems = new ObservableCollection<CalendarItem>();

            var startOffset = GetStartOffset();
            var endOffset = GetEndOffset(startOffset);

            foreach (var date in GetStartOffsetDates(startOffset)) CalendarItems.Add(new CalendarItem { Date = date });
            foreach (var date in GetDaysInMonth()) CalendarItems.Add(new CalendarItem { Date = date });
            foreach (var date in GetEndOffsetDates(endOffset)) CalendarItems.Add(new CalendarItem { Date = date });
        }

        private int GetStartOffset()
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(_currentYear, _currentMonth))
                .Select(day => ((int)new DateTime(_currentYear, _currentMonth, day).DayOfWeek + 6) % 7)
                .First();
        }

        private int GetEndOffset(int startOffset)
        {
            return (ColumnCount * RowCount) - GetDaysInMonth().Count - startOffset;
        }

        private IList<DateTime> GetStartOffsetDates(int startOffset)
        {
            IList<DateTime> dates = new List<DateTime>();
            var year = _currentYear;
            var month = _currentMonth - 1;

            if (month <= 0)
            {
                year--;
                month = 12;
            }

            dates = GetDaysInMonth(year, month);

            return dates.Reverse().Take(startOffset).Reverse().ToList();
        }

        private IList<DateTime> GetEndOffsetDates(int endOffset)
        {
            IList<DateTime> dates = new List<DateTime>();
            var year = _currentYear;
            var month = _currentMonth + 1;

            if (month > 12)
            {
                year++;
                month = 1;
            }

            dates = GetDaysInMonth(year, month);

            return dates.Take(endOffset).ToList();
        }

        private IList<DateTime> GetDaysInMonth()
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(_currentYear, _currentMonth))
                .Select(day => new DateTime(_currentYear, _currentMonth, day))
                .ToList();
        }

        private IList<DateTime> GetDaysInMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                .Select(day => new DateTime(year, month, day))
                .ToList();
        }

        public class CalendarItem : BindableBase
        {
            private DateTime? _date;

            public string DisplayDate => _date != null ? ((DateTime)_date).ToString("%d MMM yyyy") : null;

            public DateTime? Date
            {
                get => _date;
                set => SetProperty(ref _date, value);
            }
        }
    }
}
