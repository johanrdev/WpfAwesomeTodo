using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Validation;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AwesomeTodo.Module.Calendar.Dialogs
{
    public class EditCalendarEventDialogViewModel : ValidatableBindableBase, IDialogAware
    {
        private int _id;
        private string _eventTitle = string.Empty;
        private string _startTime = string.Empty;
        private string _endTime  = string.Empty;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
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

        public string Title => "Edit event";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand UpdateCalendarEventCommand { get; }

        public EditCalendarEventDialogViewModel()
        {
            UpdateCalendarEventCommand = new DelegateCommand(
                ExecuteUpdateCalendarEventCommand, 
                CanExecuteUpdateCalendarEventCommand
            )
            .ObservesProperty(() => EventTitle)
            .ObservesProperty(() => StartTime)
            .ObservesProperty(() => EndTime)
            .ObservesProperty(() => HasErrors);
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
            if (parameters.ContainsKey("CalendarEvent"))
            {
                var item = parameters.GetValue<CalendarEvent>("CalendarEvent");

                var startTime = item.StartTime.ToString("HH:mm");
                var endTime = item.EndTime.ToString("HH:mm");

                Id = item.Id;
                EventTitle = item.Title;
                StartTime = startTime;
                EndTime = endTime;
            }
        }

        private void ExecuteUpdateCalendarEventCommand()
        {
            var calendarEvent = new CalendarEvent();
            calendarEvent.Id = Id;
            calendarEvent.Title = EventTitle;
            calendarEvent.StartTime = DateTime.Parse(StartTime);
            calendarEvent.EndTime = DateTime.Parse(EndTime);

            var param = new DialogParameters();
            param.Add("CalendarEvent", calendarEvent);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
        }

        private bool CanExecuteUpdateCalendarEventCommand()
        {
            return EventTitle.Length >= 3
                && StartTime.Length >= 5
                && EndTime.Length >= 5
                && !HasErrors;
        }
    }
}
