using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Constants;
using AwesomeTodo.Shared.Validation;
using Prism.Commands;
using Prism.Regions;
using System.ComponentModel.DataAnnotations;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    internal class AddTodoViewModel : ValidatableBindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private string _title = string.Empty;

        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand AddTodoCommand { get; }

        public AddTodoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            AddTodoCommand = new DelegateCommand(ExecuteAddTodoCommand, CanExecuteAddTodoCommand)
                .ObservesProperty(() => Title)
                .ObservesProperty(() => HasErrors);
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

        private bool CanExecuteAddTodoCommand()
        {
            return !HasErrors;
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
