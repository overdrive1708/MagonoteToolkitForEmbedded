using MagonoteToolkitForEmbedded.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Input;

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

        /// <summary>
        /// キー入力コマンド
        /// </summary>
        private DelegateCommand<string> _commandKeyInput;
        public DelegateCommand<string> CommandKeyInput =>
            _commandKeyInput ?? (_commandKeyInput = new DelegateCommand<string>(ExecuteCommandKeyInput));

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 画面遷移管理情報
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// EventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            // 画面遷移管理情報を設定する
            _regionManager = regionManager;

            // 初期画面を設定する
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.UserControlWelcome));

            // EventAggregatorを保存する
            _eventAggregator = eventAggregator;
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
                case "UserControlFileInspection":
                    Title = $"{Properties.Resources.ApplicationTitle} | {Properties.Resources.ViewTitleFileInspection}";
                    break;
                case "UserControlTallyCounter":
                    Title = $"{Properties.Resources.ApplicationTitle} | {Properties.Resources.ViewTitleTallyCounter}";
                    break;
                case "UserControlAboutInfo":
                    Title = $"{Properties.Resources.ApplicationTitle} | {Properties.Resources.ViewTitleAboutInfo}";
                    break;
                default:
                    Title = Properties.Resources.ApplicationTitle;
                    break;
            }
        }

        /// <summary>
        /// キー入力コマンド実行処理
        /// </summary>
        /// <param name="inputKeyString">入力されたキー</param>
        private void ExecuteCommandKeyInput(string inputKeyString)
        {
            // Viewからの情報をKey列挙型に変換してキー入力イベントを発行する
            Key inputKey = inputKeyString switch
            {
                "NumPad0" => Key.NumPad0,
                "NumPad1" => Key.NumPad1,
                "NumPad2" => Key.NumPad2,
                "NumPad3" => Key.NumPad3,
                "NumPad4" => Key.NumPad4,
                "NumPad5" => Key.NumPad5,
                "D1" => Key.D1,
                "D2" => Key.D2,
                "D3" => Key.D3,
                "D4" => Key.D4,
                "D5" => Key.D5,
                "S" => Key.S,
                "W" => Key.W,
                "Back" => Key.Back,
                "Add" => Key.Add,
                "Subtract" => Key.Subtract,
                _ => throw new InvalidOperationException()
            };
            _eventAggregator.GetEvent<KeyInputEvent>().Publish(inputKey);
        }
    }
}
