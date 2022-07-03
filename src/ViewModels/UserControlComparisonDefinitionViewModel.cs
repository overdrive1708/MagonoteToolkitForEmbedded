using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Prism.Mvvm;
using Prism.Commands;
using MagonoteToolkitForEmbedded.Models;

namespace MagonoteToolkitForEmbedded.ViewModels
{
    public class UserControlComparisonDefinitionViewModel : BindableBase
    {
        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// 定義リストデータ
        /// </summary>
        private DefineList _defineListData = new();
        public DefineList DefineListData
        {
            get { return _defineListData; }
            set { SetProperty(ref _defineListData, value); }
        }

        /// <summary>
        /// 比較実行ボタンテキスト
        /// </summary>
        private string _compareState = Properties.Resources.ComparisonDefinitionCompare;
        public string CompareState
        {
            get { return _compareState; }
            set { SetProperty(ref _compareState, value); }
        }

        /// <summary>
        /// 比較実行ボタン有効フラグ
        /// </summary>
        private bool _isEnableCompare = true;
        public bool IsEnableCompare
        {
            get { return _isEnableCompare; }
            set { SetProperty(ref _isEnableCompare, value); }
        }

        /// <summary>
        /// 比較実行ボタンプログレスバー表示フラグ
        /// </summary>
        private bool _isProgressIndicatorVisible = false;
        public bool IsProgressIndicatorVisible
        {
            get { return _isProgressIndicatorVisible; }
            set { SetProperty(ref _isProgressIndicatorVisible, value); }
        }

        //--------------------------------------------------
        // バインディングコマンド(スニペット:cmd/cmdg)
        //--------------------------------------------------
        /// <summary>
        /// 比較実行コマンド
        /// </summary>
        private DelegateCommand _commandCompare;
        public DelegateCommand CommandCompare =>
            _commandCompare ?? (_commandCompare = new DelegateCommand(ExecuteCommandCompare));

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 比較完了後操作有効待ちタイマ
        /// </summary>
        private DispatcherTimer _timerCompareEnableWait;

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlComparisonDefinitionViewModel()
        {
            _timerCompareEnableWait = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _timerCompareEnableWait.Tick += new EventHandler(TimeoutCompareEnableWait);
        }

        /// <summary>
        /// 比較実行コマンド実行処理
        /// </summary>
        private async void ExecuteCommandCompare()
        {
            IsEnableCompare = false;
            IsProgressIndicatorVisible = true;

            CompareState = Properties.Resources.ComparisonDefinitionProcessing;

            await Task.Run(() =>
            {
                DefineListData.Compare(DefineList.CompareMode.General);
            });

            CompareState = Properties.Resources.ComparisonDefinitionComplete;

            IsProgressIndicatorVisible = false;

            _timerCompareEnableWait.Start();
        }

        /// <summary>
        /// 比較完了後操作有効待ちタイマのタイムアウト処理
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void TimeoutCompareEnableWait(object sender, EventArgs e)
        {
            _timerCompareEnableWait.Stop();

            CompareState = Properties.Resources.ComparisonDefinitionCompare;

            IsEnableCompare = true;
        }
    }
}
