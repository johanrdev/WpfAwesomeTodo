using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Constants;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    internal class AddTodoViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand AddTodoCommand { get; }

        public AddTodoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            AddTodoCommand = new DelegateCommand(ExecuteAddTodoCommand);
        }

        private void ExecuteAddTodoCommand()
        {
            using (var ctx = new AwesomeTodoDbContext())
            {
                ctx.Todos.Add(new TodoItem { Title = Title, IsCompleted = false });
                ctx.SaveChanges();
            }

            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.TodosListView);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}
