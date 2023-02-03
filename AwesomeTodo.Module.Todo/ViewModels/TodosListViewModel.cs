using AwesomeTodo.DataAccess.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections;
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
            Todos.Add(new TodoItem { Id = 1, Title = "My first todo" });
            Todos.Add(new TodoItem { Id = 2, Title = "My second todo" });
            Todos.Add(new TodoItem { Id = 3, Title = "My third todo" });
            Todos.Add(new TodoItem { Id = 4, Title = "My fourth todo" });
            Todos.Add(new TodoItem { Id = 5, Title = "My fifth todo" });
            SelectedTodo = Todos.First();

            GotoAddTodoViewCommand = new DelegateCommand(ExecuteGotoAddTodoViewCommand, CanExecuteGotoAddTodoViewCommand);
            RemoveTodosCommand = new DelegateCommand<object>(ExecuteRemoveTodosCommand, CanExecuteRemoveTodosCommand)
                    .ObservesProperty(() => SelectedTodo);
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
            var items = ((IList)obj).OfType<TodoItem>().ToList();
            
            foreach (var item in items)
            {
                var todo = Todos.Where(t => t.Id == item.Id).First();

                Todos.Remove(todo);
            }
        }

        private bool CanExecuteRemoveTodosCommand(object obj)
        {
            return SelectedTodo != null;
        }

        private void ExecuteResetTodosCommand()
        {
            
        }

        private bool CanExecuteResetTodosCommand()
        {
            return true;
        }
    }
}
