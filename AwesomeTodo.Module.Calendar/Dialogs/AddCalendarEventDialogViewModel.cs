using Prism.Services.Dialogs;
using System;
using System.Diagnostics;

namespace AwesomeTodo.Module.Calendar.Dialogs
{
    public class AddCalendarEventDialogViewModel : IDialogAware
    {
        public string Title => "Add event";

        public event Action<IDialogResult> RequestClose;

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
            Debug.WriteLine("Opened dialog");
        }
    }
}
