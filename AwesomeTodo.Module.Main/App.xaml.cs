using AwesomeTodo.Module.Main.Views;
using AwesomeTodo.Module.Todo;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace AwesomeTodo.Module.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<TodoModule>();
        }
    }
}
