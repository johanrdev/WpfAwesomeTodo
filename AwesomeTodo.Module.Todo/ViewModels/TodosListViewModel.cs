using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
            GotoAddTodoViewCommand = new DelegateCommand(ExecuteGotoAddTodoViewCommand, CanExecuteGotoAddTodoViewCommand);
            RemoveTodosCommand = new DelegateCommand<object>(ExecuteRemoveTodosCommand, CanExecuteRemoveTodosCommand)
                    .ObservesProperty(() => SelectedTodo);
            ResetTodosCommand = new DelegateCommand(ExecuteResetTodosCommand, CanExecuteResetTodosCommand);

            LoadTodos().Await();
        }

        private async Task LoadTodos()
        {
            List<TodoItem> todos = new List<TodoItem>();

            Todos.Clear();

            using (var ctx = new AwesomeTodoDbContext())
            {
                todos = await ctx.Todos.ToListAsync();
            }

            if (todos.Count > 0)
            {
                foreach (var todo in todos)
                {
                    Todos.Add(todo);
                }

                SelectedTodo = Todos.First();
            }
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

            using (var ctx = new AwesomeTodoDbContext())
            {
                foreach (var item in items)
                {
                    var todo = ctx.Todos.Where(t => t.Id == item.Id).FirstOrDefault();

                    if (todo != null)
                    {
                        ctx.Todos.Remove(todo);
                    }

                    ctx.SaveChanges();
                }
            }

            LoadTodos().Await();
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
