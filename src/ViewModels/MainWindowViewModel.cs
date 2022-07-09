using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;

namespace MagonoteToolkitForEmbedded.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// タイトル
        /// </summary>
        private string _title = Properties.Resources.ApplicationTitle;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        //--------------------------------------------------
        // バインディングコマンド(スニペット:cmd/cmdg)
        //--------------------------------------------------
        /// <summary>
        /// 画面遷移コマンド
        /// </summary>
        private DelegateCommand<string> _commandTransitionView;
        public DelegateCommand<string> CommandTransitionView =>
            _commandTransitionView ?? (_commandTransitionView = new DelegateCommand<string>(ExecuteCommandTransitionView));

        //--------------------------------------------------
        // 変数
        //--------------------------------------------------
        /// <summary>
        /// 画面遷移管理情報
        /// </summary>
        private readonly IRegionManager _regionManager;

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        public MainWindowViewModel(IRegionManager regionManager)
        {
            // 画面遷移管理情報を設定する
            _regionManager = regionManager;

            // 初期画面を設定する
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.UserControlWelcome));

        }

        /// <summary>
        /// 画面遷移コマンド実行処理
        /// </summary>
        /// <param name="viewname">画面遷移先のユーザーコントロール名</param>
        private void ExecuteCommandTransitionView(string viewname)
        {
            // 指定された画面に遷移する
            _regionManager.RequestNavigate("ContentRegion", viewname);

            // タイトルを更新する
            switch (viewname)
            {
                case "UserControlComparisonDefinition":
                    Title = $"{Properties.Resources.ApplicationTitle} | {Properties.Resources.ViewTitleComparisonDefinition}";
                    break;
                case "UserControlAboutInfo":
                    Title = $"{Properties.Resources.ApplicationTitle} | {Properties.Resources.ViewTitleAboutInfo}";
                    break;
                default:
                    break;
            }
        }
    }
}
