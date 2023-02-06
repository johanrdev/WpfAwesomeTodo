using AwesomeTodo.Module.Calendar.Dialogs;
using AwesomeTodo.Module.Calendar.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace AwesomeTodo.Module.Calendar
{
    public class CalendarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CalendarView>();

            containerRegistry.RegisterDialog<AddCalendarEventDialog>();
            containerRegistry.RegisterDialog<EditCalendarEventDialog>();
        }
    }
}
