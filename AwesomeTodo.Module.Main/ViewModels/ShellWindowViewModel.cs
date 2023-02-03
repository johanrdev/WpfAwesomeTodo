using AwesomeTodo.Shared.Constants;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;

namespace AwesomeTodo.Module.Main.ViewModels
{
    internal class ShellWindowViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        public string Title => "AwesomeTodo";
        public DelegateCommand GotoPreviousViewCommand { get; }
        public DelegateCommand GotoNextViewCommand { get; }
        public DelegateCommand ExitApplicationCommand { get; }

        public ShellWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            GotoPreviousViewCommand = new DelegateCommand(ExecuteGotoPreviousViewCommand);
            GotoNextViewCommand = new DelegateCommand(ExecuteGotoNextViewCommand);
            ExitApplicationCommand = new DelegateCommand(ExecuteExitApplicationCommand);
        }

        private void ExecuteGotoPreviousViewCommand()
        {
            var contentRegion = _regionManager.Regions[RegionNames.ContentRegion];

            contentRegion.NavigationService.Journal.GoBack();
        }

        private bool CanGotoPreviousView()
        {
            var contentRegion = _regionManager.Regions[RegionNames.ContentRegion];

            return contentRegion.NavigationService.Journal.CanGoBack;
        }

        private void ExecuteGotoNextViewCommand()
        {
            var contentRegion = _regionManager.Regions[RegionNames.ContentRegion];

            contentRegion.NavigationService.Journal.GoForward();
        }

        private bool CanGotoNextView()
        {
            var contentRegion = _regionManager.Regions[RegionNames.ContentRegion];

            return contentRegion.NavigationService.Journal.CanGoForward;
        }

        private void ExecuteExitApplicationCommand()
        {
            App.Current.Shutdown();
        }
    }
}
