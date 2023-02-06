using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Validation;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel.DataAnnotations;

namespace AwesomeTodo.Module.Calendar.Dialogs
{
    public class AddCalendarEventDialogViewModel : ValidatableBindableBase, IDialogAware
    {
        private DateTime _selectedDate = new DateTime();
        private string _eventTitle = string.Empty;
        private string _startTime = string.Empty;
        private string _endTime = string.Empty;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string EventTitle
        {
            get => _eventTitle;
            set => SetProperty(ref _eventTitle, value);
        }

        [Required]
        [RegularExpression(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")]
        public string StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        [Required]
        [RegularExpression(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")]
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
            AddCalendarEventCommand = new DelegateCommand(
                ExecuteAddCalendarEventCommand,
                CanExecuteAddCalendarEventCommand
            )
            .ObservesProperty(() => EventTitle)
            .ObservesProperty(() => StartTime)
            .ObservesProperty(() => EndTime)
            .ObservesProperty(() => HasErrors);
        }

        private void ExecuteAddCalendarEventCommand()
        {
            var startTimeArray = DateTime.Parse(StartTime);
            var endTimeArray = DateTime.Parse(EndTime);

            CalendarEvent calendarEvent = new CalendarEvent();
            calendarEvent.Title = EventTitle;
            calendarEvent.StartTime = new DateTime(
                SelectedDate.Year, 
                SelectedDate.Month, 
                SelectedDate.Day,
                startTimeArray.Hour, 
                startTimeArray.Minute, 
                startTimeArray.Second
            );
            calendarEvent.EndTime = new DateTime(
                SelectedDate.Year, 
                SelectedDate.Month, 
                SelectedDate.Day, 
                endTimeArray.Hour, 
                endTimeArray.Minute, 
                endTimeArray.Second
            );

            var param = new DialogParameters();
            param.Add("CalendarEvent", calendarEvent);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
        }

        private bool CanExecuteAddCalendarEventCommand()
        {
            return EventTitle.Length >= 3 
                && StartTime.Length >= 5 
                && EndTime.Length >= 5 
                && !HasErrors;
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
