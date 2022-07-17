using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Prism.Mvvm;
using Prism.Commands;
using MagonoteToolkitForEmbedded.Models;
using System.Windows;
using System.Windows.Data;

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
        private string _compareState = Properties.Resources.Compare;
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
        /// 入力有効フラグ
        /// </summary>
        private bool _isEnableInput = true;
        public bool IsEnableInput
        {
            get { return _isEnableInput; }
            set { SetProperty(ref _isEnableInput, value); }
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

        /// <summary>
        /// C言語define比較フラグ
        /// </summary>
        private bool _isClangDefineMode;
        public bool IsClangDefineMode
        {
            get { return _isClangDefineMode; }
            set { SetProperty(ref _isClangDefineMode, value); }
        }

        /// <summary>
        /// 変更定義表示フラグ
        /// </summary>
        private Visibility _changeDefineListVisibility = Visibility.Collapsed;
        public Visibility ChangeDefineListVisibility
        {
            get { return _changeDefineListVisibility; }
            set { SetProperty(ref _changeDefineListVisibility, value); }
        }

        /// <summary>
        /// 定義リストの高さ
        /// </summary>
        private int _defineListHeight = 500;
        public int DefineListHeight
        {
            get { return _defineListHeight; }
            set { SetProperty(ref _defineListHeight, value); }
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

        /// <summary>
        /// C言語define比較チェックボックス変更コマンド
        /// </summary>
        private DelegateCommand _commandChangeClangDefineMode;
        public DelegateCommand CommandChangeClangDefineMode =>
            _commandChangeClangDefineMode ?? (_commandChangeClangDefineMode = new DelegateCommand(ExecuteCommandChangeClangDefineMode));

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

            BindingOperations.EnableCollectionSynchronization(DefineListData.ChangeDefines, new object());
        }

        /// <summary>
        /// 比較実行コマンド実行処理
        /// </summary>
        private async void ExecuteCommandCompare()
        {
            IsEnableCompare = false;
            IsEnableInput = false;
            IsProgressIndicatorVisible = true;

            CompareState = Properties.Resources.MessageProcessing;

            await Task.Run(() =>
            {
                if (IsClangDefineMode)
                {
                    DefineListData.Compare(DefineList.CompareMode.ClangDefine);
                }
                else
                {
                    DefineListData.Compare(DefineList.CompareMode.General);
                }
            });

            CompareState = Properties.Resources.MessageComplete;

            IsEnableInput = true;
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

            CompareState = Properties.Resources.Compare;

            IsEnableCompare = true;
        }

        /// <summary>
        /// C言語define比較チェックボックス変更コマンド実行処理
        /// </summary>
        private void ExecuteCommandChangeClangDefineMode()
        {
            if (IsClangDefineMode)
            {
                ChangeDefineListVisibility = Visibility.Visible;
                DefineListHeight = 300;
            }
            else
            {
                ChangeDefineListVisibility = Visibility.Collapsed;
                DefineListHeight = 500;
            }
        }
    }
}
