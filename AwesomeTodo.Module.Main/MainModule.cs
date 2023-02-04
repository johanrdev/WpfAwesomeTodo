using AwesomeTodo.Module.Main.Views;
using AwesomeTodo.Shared.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AwesomeTodo.Module.Main
{
    internal class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RequestNavigate(RegionNames.NavigationRegion, ViewNames.NavigationView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationView>();
        }
    }
}
