using Prism.Mvvm;
using System;

namespace AwesomeTodo.DataAccess.Models
{
    public class CalendarEvent : BindableBase
    {
        private int _id;
        private string _title;
        private DateTime _startTime;
        private DateTime _endTime;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DateTime StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public DateTime EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }
    }
}
