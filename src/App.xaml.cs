using MagonoteToolkitForEmbedded.Views;
using Prism.Ioc;
using System.Windows;

namespace MagonoteToolkitForEmbedded
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 画面遷移を行う画面を登録する
            containerRegistry.RegisterForNavigation<UserControlWelcome>();
            containerRegistry.RegisterForNavigation<UserControlAboutInfo>();
        }
    }
}
