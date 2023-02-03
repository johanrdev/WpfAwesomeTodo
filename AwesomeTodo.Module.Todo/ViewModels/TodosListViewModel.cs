using AwesomeTodo.DataAccess.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
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

        public TodosListViewModel()
        {
            Todos = new ObservableCollection<TodoItem>();
            Todos.Add(new TodoItem { Title = "My first todo" });
            Todos.Add(new TodoItem { Title = "My second todo" });
            Todos.Add(new TodoItem { Title = "My third todo" });
            Todos.Add(new TodoItem { Title = "My fourth todo" });
            Todos.Add(new TodoItem { Title = "My fifth todo" });
            SelectedTodo = Todos.First();
        }
    }
}
