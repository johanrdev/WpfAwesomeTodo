using AwesomeTodo.DataAccess;
using AwesomeTodo.DataAccess.Models;
using AwesomeTodo.Shared.Constants;
using AwesomeTodo.Shared.Validation;
using Prism.Commands;
using Prism.Regions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    public class EditTodoViewModel : ValidatableBindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private int _id;
        private string _title = string.Empty;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand UpdateTodoCommand { get; }

        public EditTodoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            UpdateTodoCommand = new DelegateCommand(ExecuteUpdateTodoCommand, CanExecuteUpdateTodoCommand)
                .ObservesProperty(() => Title)
                .ObservesProperty(() => HasErrors);
        }

        private void ExecuteUpdateTodoCommand()
        {
            using (var ctx = new AwesomeTodoDbContext())
            {
                var todo = ctx.Todos.Where(t => t.Id == Id).FirstOrDefault();
                todo.Title = Title;

                ctx.SaveChanges();

                _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.TodosListView);
            }
        }

        private bool CanExecuteUpdateTodoCommand()
        {
            return Title.Length >= 3 && !HasErrors;
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
            var item = navigationContext.Parameters.GetValue<TodoItem>("TodoItem");

            Id = item.Id;
            Title = item.Title;
        }
    }
}
