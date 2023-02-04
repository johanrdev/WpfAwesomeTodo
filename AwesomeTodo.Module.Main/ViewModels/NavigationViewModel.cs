using AwesomeTodo.Shared.Constants;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Diagnostics;

namespace AwesomeTodo.Module.Main.ViewModels
{
    internal class NavigationViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; }

        public NavigationViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand);
        }

        private void ExecuteNavigateCommand(string path)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, path);
        }
    }
}
