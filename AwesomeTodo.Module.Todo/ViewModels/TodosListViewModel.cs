using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Constants;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    internal class TodosListViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private TodoItem _selectedTodo;

        public TodoItem SelectedTodo
        {
            get => _selectedTodo;
            set => SetProperty(ref _selectedTodo, value);
        }

        public ObservableCollection<TodoItem> Todos { get; }
        public DelegateCommand<TodoItem> ToggleTodoCompletionCommand { get; }
        public DelegateCommand GotoAddTodoViewCommand { get; }
        public DelegateCommand<object> RemoveTodosCommand { get; }
        public DelegateCommand ResetTodosCommand { get; }

        public TodosListViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            Todos = new ObservableCollection<TodoItem>();
            ToggleTodoCompletionCommand = new DelegateCommand<TodoItem>(ExecuteToggleTodoCompletionCommand);
            GotoAddTodoViewCommand = new DelegateCommand(ExecuteGotoAddTodoViewCommand, CanExecuteGotoAddTodoViewCommand);
            RemoveTodosCommand = new DelegateCommand<object>(ExecuteRemoveTodosCommand, CanExecuteRemoveTodosCommand)
                .ObservesProperty(() => SelectedTodo);
            ResetTodosCommand = new DelegateCommand(ExecuteResetTodosCommand, CanExecuteResetTodosCommand)
                .ObservesProperty(() => Todos.Count);

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

        private void ExecuteToggleTodoCompletionCommand(TodoItem obj)
        {
            using (var ctx = new AwesomeTodoDbContext())
            {
                var todo = ctx.Todos.Find(obj.Id);
                todo.IsCompleted = obj.IsCompleted;
                ctx.SaveChanges();
            }

            RaisePropertyChanged(nameof(Todos));
        }

        private void ExecuteGotoAddTodoViewCommand()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.AddTodoView);
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
            using (var ctx = new AwesomeTodoDbContext()) 
            {
                var todos = ctx.Todos.Where(t => t.IsCompleted);

                foreach (var todo in todos) 
                {
                    todo.IsCompleted = false;
                }

                ctx.SaveChanges();
            }

            LoadTodos().Await();
        }

        private bool CanExecuteResetTodosCommand()
        {
            return Todos.Count(t => t.IsCompleted) > 0;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
