using AwesomeTodo.DataAccess.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Diagnostics;
using System.Linq;

namespace AwesomeTodo.Module.Calendar.Dialogs
{
    public class AddCalendarEventDialogViewModel : BindableBase, IDialogAware
    {
        private DateTime _selectedDate;
        private string _eventTitle;
        private string _startTime;
        private string _endTime;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public string EventTitle
        {
            get => _eventTitle;
            set => SetProperty(ref _eventTitle, value);
        }

        public string StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public string EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }

        public string Title => "Add event";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand AddCalendarEventCommand { get; }


        public AddCalendarEventDialogViewModel()
        {
            AddCalendarEventCommand = new DelegateCommand(ExecuteAddCalendarEventCommand);
        }

        private void ExecuteAddCalendarEventCommand()
        {
            var startTimeArray = DateTime.Parse(StartTime);
            var endTimeArray = DateTime.Parse(EndTime);

            CalendarEvent calendarEvent = new CalendarEvent();
            calendarEvent.Title = EventTitle;
            calendarEvent.StartTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, startTimeArray.Hour, startTimeArray.Minute, startTimeArray.Second);
            calendarEvent.EndTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, endTimeArray.Hour, endTimeArray.Minute, endTimeArray.Second);

            var param = new DialogParameters();
            param.Add("CalendarEvent", calendarEvent);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.ContainsKey("Date"))
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            }

            SelectedDate = parameters.GetValue<DateTime>("Date");
        }
    }
}
