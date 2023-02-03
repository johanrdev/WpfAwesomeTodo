using Prism.Mvvm;
using Prism.Regions;

namespace AwesomeTodo.Module.Todo.ViewModels
{
    internal class AddTodoViewModel : BindableBase, INavigationAware
    {
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
