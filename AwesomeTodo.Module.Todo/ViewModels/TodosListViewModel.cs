using AwesomeTodo.DataAccess.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    internal class TodosListViewModel : BindableBase
    {
        private TodoItem _selectedTodo;

        public TodoItem SelectedTodo
        {
            get => _selectedTodo;
            set => SetProperty(ref _selectedTodo, value);
        }

        public ObservableCollection<TodoItem> Todos { get; }
        public DelegateCommand GotoAddTodoViewCommand { get; }
        public DelegateCommand<object> RemoveTodosCommand { get; }
        public DelegateCommand ResetTodosCommand { get; }

        public TodosListViewModel()
        {
            Todos = new ObservableCollection<TodoItem>();
            Todos.Add(new TodoItem { Title = "My first todo" });
            Todos.Add(new TodoItem { Title = "My second todo" });
            Todos.Add(new TodoItem { Title = "My third todo" });
            Todos.Add(new TodoItem { Title = "My fourth todo" });
            Todos.Add(new TodoItem { Title = "My fifth todo" });
            SelectedTodo = Todos.First();

            GotoAddTodoViewCommand = new DelegateCommand(ExecuteGotoAddTodoViewCommand, CanExecuteGotoAddTodoViewCommand);
            RemoveTodosCommand = new DelegateCommand<object>(ExecuteRemoveTodosCommand, CanExecuteRemoveTodosCommand);
            ResetTodosCommand = new DelegateCommand(ExecuteResetTodosCommand, CanExecuteResetTodosCommand);
        }

        private void ExecuteGotoAddTodoViewCommand()
        {
            Debug.WriteLine("Goto add todo view");
        }

        private bool CanExecuteGotoAddTodoViewCommand()
        {
            return true;
        }

        private void ExecuteRemoveTodosCommand(object obj)
        {
            Debug.WriteLine("Remove todos:");
            Debug.WriteLine(obj);
        }

        private bool CanExecuteRemoveTodosCommand(object obj)
        {
            return true;
        }

        private void ExecuteResetTodosCommand()
        {
            Debug.WriteLine("Reset todos");
        }

        private bool CanExecuteResetTodosCommand()
        {
            return true;
        }
    }
}
