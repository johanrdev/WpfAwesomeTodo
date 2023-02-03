using AwesomeTodo.Module.Todo.Views;
using AwesomeTodo.Shared.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AwesomeTodo.Module.Todo
{
    public class TodoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(TodosListView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TodosListView>();
            containerRegistry.RegisterForNavigation<AddTodoView>();
        }
    }
}
