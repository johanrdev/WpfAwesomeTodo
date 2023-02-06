using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Module.Calendar.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AwesomeTodo.Module.Calendar.ViewModels
{
    internal class CalendarViewModel : BindableBase 
    {
        private IDialogService _dialogService;
        private int _currentYear = DateTime.Now.Year;
        private int _currentMonth = DateTime.Now.Month;
        private IList<int> _years = new List<int>();
        private IList<string> _months = new List<string>();
        private int _selectedYear;
        private string _selectedMonth;
        private CalendarItem _selectedCalendarItem;

        public int ColumnCount => 7;
        public int RowCount => 6;

        public int CurrentYear
        {
            get => _currentYear;
            set => SetProperty(ref _currentYear, value);
        }

        public int CurrentMonth
        {
            get => _currentMonth;
            set => SetProperty(ref _currentMonth, value);
        }

        public IList<int> Years
        {
            get => _years;
            set => SetProperty(ref _years, value);
        }

        public IList<string> Months
        {
            get => _months;
            set => SetProperty(ref _months, value);
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set => SetProperty(ref _selectedYear, value);
        }

        public string SelectedMonth
        {
            get => _selectedMonth;
            set => SetProperty(ref _selectedMonth, value);
        }

        public CalendarItem SelectedCalendarItem
        {
            get => _selectedCalendarItem;
            set => SetProperty(ref _selectedCalendarItem, value);
        }

        public ObservableCollection<CalendarItem> CalendarItems { get; private set; }
        public DelegateCommand SelectYearCommand { get; }
        public DelegateCommand SelectMonthCommand { get; }
        public DelegateCommand<CalendarItem> OpenAddCalendarEventCommand { get; }
        public DelegateCommand<CalendarItem> SelectCalendarItemCommand { get; }
        public DelegateCommand<CalendarEvent> RemoveCalendarEventCommand { get; }

        public CalendarViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            CalendarItems = new ObservableCollection<CalendarItem>();
            SelectYearCommand = new DelegateCommand(ExecuteSelectYearCommand);
            SelectMonthCommand = new DelegateCommand(ExecuteSelectMonthCommand);
            OpenAddCalendarEventCommand = new DelegateCommand<CalendarItem>(ExecuteOpenAddCalendarEventCommand);
            SelectCalendarItemCommand = new DelegateCommand<CalendarItem>(ExecuteSelectCalendarItemCommand);
            RemoveCalendarEventCommand = new DelegateCommand<CalendarEvent>(ExecuteRemoveCalendarEventCommand);

            PopulateYears();
            PopulateMonths();
            InitCalendar();
        }

        private void ExecuteSelectYearCommand()
        {
            CurrentYear = SelectedYear;

            InitCalendar();
        }

        private void ExecuteSelectMonthCommand()
        {
            CurrentMonth = DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;

            InitCalendar();
        }

        private void ExecuteOpenAddCalendarEventCommand(CalendarItem item)
        {
            var param = new DialogParameters();
            param.Add("Date", item.Date);

            _dialogService.ShowDialog(nameof(AddCalendarEventDialog), param, callback => 
            { 
                if (callback.Result == ButtonResult.OK)
                {
                    if (callback.Parameters.ContainsKey("CalendarEvent"))
                    {
                        var newCalendarEvent = callback.Parameters.GetValue<CalendarEvent>("CalendarEvent");

                        using (var ctx = new AwesomeTodoDbContext())
                        {
                            ctx.CalendarEvents.Add(newCalendarEvent);
                            ctx.SaveChanges();
                        }

                        LoadCalendarEvents();

                        InitCalendar();

                        SelectedCalendarItem = CalendarItems.Where(c => c.Date == newCalendarEvent.StartTime.Date).First();
                    }
                }
            });
        }

        private void ExecuteSelectCalendarItemCommand(CalendarItem item)
        {
            Debug.WriteLine(item);
        }

        private void ExecuteRemoveCalendarEventCommand(CalendarEvent item)
        {
            using (var ctx = new AwesomeTodoDbContext())
            {
                var calendarEvent = ctx.CalendarEvents.Where(c => c.Id == item.Id).FirstOrDefault();
                ctx.CalendarEvents.Remove(calendarEvent);
                ctx.SaveChanges();
            }

            LoadCalendarEvents();

            InitCalendar();

            SelectedCalendarItem = CalendarItems.Where(c => c.Date == item.StartTime.Date).First();
        }

        private void PopulateYears()
        {
            Years = Enumerable.Range(DateTime.Now.Year, 5).ToList();

            SelectedYear = Years.First();
        }

        private void PopulateMonths()
        {
            Months = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();

            SelectedMonth = Months.Where(m => m.Equals(Months[DateTime.Now.Month - 1])).First();
        }

        private void InitCalendar()
        {
            CalendarItems.Clear();

            var startOffset = GetStartOffset();
            var endOffset = GetEndOffset(startOffset);

            foreach (var date in GetStartOffsetDates(startOffset)) CalendarItems.Add(new CalendarItem(_currentMonth) { Date = date });
            foreach (var date in GetDaysInMonth()) CalendarItems.Add(new CalendarItem(_currentMonth) { Date = date });
            foreach (var date in GetEndOffsetDates(endOffset)) CalendarItems.Add(new CalendarItem(_currentMonth) { Date = date });

            LoadCalendarEvents();
        }

        private void LoadCalendarEvents()
        {
            using (var ctx = new AwesomeTodoDbContext())
            {
                foreach (var item in CalendarItems)
                {
                    var events = ctx.CalendarEvents.Where(c => DbFunctions.TruncateTime(c.StartTime) == item.Date).OrderBy(c => c.StartTime);

                    item.Events = new ObservableCollection<CalendarEvent>(events);
                }
            }

            SelectedCalendarItem = CalendarItems.Where(c => c.Date == DateTime.Today).FirstOrDefault();
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
            private int _currentMonth = DateTime.Now.Month;

            public DateTime? Date
            {
                get => _date;
                set => SetProperty(ref _date, value);
            }

            public bool HasEvents => Events.Count > 0;
            public string DisplayDate => _date != null ? ((DateTime)_date).ToString("%d") : null;
            public bool IsCurrentMonth => _date != null ? ((DateTime)_date).Month == _currentMonth : false;
            public bool IsCurrentDate => _date != null ? (DateTime)_date == DateTime.Today : false;
            public ObservableCollection<CalendarEvent> Events { get; set; }

            public CalendarItem(int month)
            {
                _currentMonth = month;

                Events = new ObservableCollection<CalendarEvent>();
            }
        }
    }
}
