using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;

namespace AwesomeTodo.MainModule.ViewModels
{
    internal class ShellWindowViewModel : BindableBase
    {
        public DelegateCommand GotoPreviousViewCommand { get; }
        public DelegateCommand GotoNextViewCommand { get; }

        public ShellWindowViewModel()
        {
            GotoPreviousViewCommand = new DelegateCommand(ExecuteGotoPreviousViewCommand);
            GotoNextViewCommand = new DelegateCommand(ExecuteGotoNextViewCommand);
        }

        private void ExecuteGotoPreviousViewCommand()
        {
            Debug.WriteLine("Prev");
        }

        private void ExecuteGotoNextViewCommand()
        {
            Debug.WriteLine("Next");
        }
    }
}
