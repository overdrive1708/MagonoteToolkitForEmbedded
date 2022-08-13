using CommandLine;
using MagonoteToolkitForEmbedded.Utilities;
using MagonoteToolkitForEmbedded.Views;
using Prism.Ioc;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MagonoteToolkitForEmbedded
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // コマンドラインオプションを解析してカルチャーを切り替える
            using (CommandLine.Parser parser = new((setting) => setting.HelpWriter = null))
            {
                CommandLine.ParserResult<CommandLineOptions> parsed = parser.ParseArguments<CommandLineOptions>(e.Args);
                _ = parsed.WithParsed(opt =>
                {
                    if (opt.CultureInfo == "en-US")
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    }
                });
            }

            base.OnStartup(e);

            // 未処理の例外を処理するイベントハンドラを登録する
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 画面遷移を行う画面を登録する
            containerRegistry.RegisterForNavigation<UserControlWelcome>();
            containerRegistry.RegisterForNavigation<UserControlComparisonDefinition>();
            containerRegistry.RegisterForNavigation<UserControlFileInspection>();
            containerRegistry.RegisterForNavigation<UserControlAboutInfo>();
        }

        /// <summary>
        /// DispatcherUnhandledExceptionイベント発生時の処理
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => HandleException(e.Exception);

        /// <summary>
        /// UnobservedTaskExceptionイベント発生時の処理
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) => HandleException(e.Exception.InnerException);

        /// <summary>
        /// UnhandledExceptionイベント発生時の処理
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) => HandleException((Exception)e.ExceptionObject);

        /// <summary>
        /// 例外発生時の処理
        /// </summary>
        /// <param name="e">例外情報</param>
        private static void HandleException(Exception e)
        {
            _ = MessageBox.Show($"{MagonoteToolkitForEmbedded.Properties.Resources.MessageFatalError}\r\n{e?.ToString()}",
                                MagonoteToolkitForEmbedded.Properties.Resources.ImportantNotice,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            Environment.Exit(1);
        }
    }
}
