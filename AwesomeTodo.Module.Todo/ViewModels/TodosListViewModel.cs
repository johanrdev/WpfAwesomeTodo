using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Constants;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    internal class TodosListViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private bool _isLoading;
        private TodoItem _selectedTodo;
        private ListCollectionView _viewSource;
        private string _filterString;

        public bool CanAcceptChildren { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public TodoItem SelectedTodo
        {
            get => _selectedTodo;
            set => SetProperty(ref _selectedTodo, value);
        }

        public ListCollectionView ViewSource
        {
            get => _viewSource;
            set => SetProperty(ref _viewSource, value);
        }

        public string FilterString
        {
            get => _filterString;
            set => SetProperty(ref _filterString, value);
        }

        public ObservableCollection<TodoItem> Todos { get; private set; }
        public DelegateCommand FilterCommand { get; }
        public DelegateCommand<TodoItem> ToggleTodoCompletionCommand { get; }
        public DelegateCommand GotoAddTodoViewCommand { get; }
        public DelegateCommand<object> RemoveTodosCommand { get; }
        public DelegateCommand ResetTodosCommand { get; }

        public TodosListViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            Todos = new ObservableCollection<TodoItem>();
            FilterCommand = new DelegateCommand(ExecuteFilterCommand);
            ToggleTodoCompletionCommand = new DelegateCommand<TodoItem>(ExecuteToggleTodoCompletionCommand);
            GotoAddTodoViewCommand = new DelegateCommand(ExecuteGotoAddTodoViewCommand, CanExecuteGotoAddTodoViewCommand);
            RemoveTodosCommand = new DelegateCommand<object>(ExecuteRemoveTodosCommand, CanExecuteRemoveTodosCommand)
                .ObservesProperty(() => SelectedTodo);
            ResetTodosCommand = new DelegateCommand(ExecuteResetTodosCommand, CanExecuteResetTodosCommand)
                .ObservesProperty(() => Todos.Count);
        }

        private void LoadTodos()
        {
            List<TodoItem> todos;

            IsLoading = true;

            Task.Run(() =>
            {
                using (var ctx = new AwesomeTodoDbContext())
                {
                    todos = ctx.Todos.ToList();
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    IsLoading = false;

                    Todos = new ObservableCollection<TodoItem>(todos);

                    SelectedTodo = Todos.FirstOrDefault();

                    InitializeViewSource();
                });
            });
        }

        private void InitializeViewSource()
        {
            ViewSource = (ListCollectionView)CollectionViewSource.GetDefaultView(Todos);
            ViewSource.Filter = ViewSourceFilter;
            ViewSource.Refresh();
        }

        private bool ViewSourceFilter(object item)
        {
            if (item is TodoItem)
            {
                var todoItem = item as TodoItem;

                return string.IsNullOrEmpty(FilterString) || string.IsNullOrWhiteSpace(FilterString) ?
                    true : todoItem.Title.ToLower().Contains(FilterString.ToLower());
            }

            return true;
        }

        private void ExecuteFilterCommand()
        {
            ViewSource.Refresh();
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

            LoadTodos();
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

            LoadTodos();
        }

        private bool CanExecuteResetTodosCommand()
        {
            return Todos.Count(t => t.IsCompleted) > 0;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadTodos();
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
