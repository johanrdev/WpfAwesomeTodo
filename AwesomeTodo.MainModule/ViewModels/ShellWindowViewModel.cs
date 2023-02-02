using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AwesomeTodo.MainModule.ViewModels
{
    internal class ShellWindowViewModel : BindableBase
    {
        private Todo _selectedTodo;
        
        public Todo SelectedTodo
        {
            get => _selectedTodo;
            set => SetProperty(ref _selectedTodo, value);
        }

        public string Title => "AwesomeTodo";
        public DelegateCommand GotoPreviousViewCommand { get; }
        public DelegateCommand GotoNextViewCommand { get; }
        public ObservableCollection<Todo> Todos { get; }

        public ShellWindowViewModel()
        {
            GotoPreviousViewCommand = new DelegateCommand(ExecuteGotoPreviousViewCommand);
            GotoNextViewCommand = new DelegateCommand(ExecuteGotoNextViewCommand);
            Todos = new ObservableCollection<Todo>();
            Todos.Add(new Todo { Title = "My first todo" });
            Todos.Add(new Todo { Title = "My second todo" });
            Todos.Add(new Todo { Title = "My third todo" });
            Todos.Add(new Todo { Title = "My fourth todo" });
            Todos.Add(new Todo { Title = "My fifth todo" });
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

    public class Todo : BindableBase
    {
        private int _id;
        private string _title;
        
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
    }
}
