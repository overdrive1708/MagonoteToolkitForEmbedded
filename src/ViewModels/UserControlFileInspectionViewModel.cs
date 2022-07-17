using MagonoteToolkitForEmbedded.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace MagonoteToolkitForEmbedded.ViewModels
{
    public class UserControlFileInspectionViewModel : BindableBase
    {
        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// 定義リストデータ
        /// </summary>
        private FileInspector _fileInspectorData = new();
        public FileInspector FileInspectorData
        {
            get { return _fileInspectorData; }
            set { SetProperty(ref _fileInspectorData, value); }
        }

        /// <summary>
        /// 重要なお知らせ
        /// </summary>
        private string _importantNotice = $"{Properties.Resources.ImportantNotice}:{Properties.Resources.MessageSupportOnlyShiftJIS}";
        public string ImportantNotice
        {
            get { return _importantNotice; }
            set { SetProperty(ref _importantNotice, value); }
        }

        /// <summary>
        /// 検査実行ボタンテキスト
        /// </summary>
        private string _inspectionState = Properties.Resources.Inspection;
        public string InspectionState
        {
            get { return _inspectionState; }
            set { SetProperty(ref _inspectionState, value); }
        }

        /// <summary>
        /// 検査実行ボタン有効フラグ
        /// </summary>
        private bool _isEnableInspection = true;
        public bool IsEnableInspection
        {
            get { return _isEnableInspection; }
            set { SetProperty(ref _isEnableInspection, value); }
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
        /// 検査実行ボタンプログレスバー表示フラグ
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
        /// 検査実行コマンド
        /// </summary>
        private DelegateCommand _commandInspection;
        public DelegateCommand CommandInspection =>
            _commandInspection ?? (_commandInspection = new DelegateCommand(ExecuteCommandInspection));

        /// <summary>
        /// フォルダ選択ダイアログ表示コマンド
        /// </summary>
        private DelegateCommand _commandShowOpenFolderDialog;
        public DelegateCommand CommandOpenShowFolderDialog =>
            _commandShowOpenFolderDialog ?? (_commandShowOpenFolderDialog = new DelegateCommand(ExecuteCommandShowOpenFolderDialog));

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 検査完了後操作有効待ちタイマ
        /// </summary>
        private readonly DispatcherTimer _timerInspectionEnableWait;

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlFileInspectionViewModel()
        {
            _timerInspectionEnableWait = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _timerInspectionEnableWait.Tick += new EventHandler(TimeoutInspectionEnableWait);

            BindingOperations.EnableCollectionSynchronization(FileInspectorData.InspectionResultData, new object());
        }
        
        /// <summary>
        /// 検査実行コマンド実行処理
        /// </summary>
        private async void ExecuteCommandInspection()
        {
            IsEnableInspection = false;
            IsEnableInput = false;
            IsProgressIndicatorVisible = true;

            InspectionState = Properties.Resources.MessageProcessing;

            await Task.Run(() =>
            {
                FileInspectorData.Inspection();
            });

            InspectionState = Properties.Resources.MessageComplete;

            IsEnableInput = true;
            IsProgressIndicatorVisible = false;

            _timerInspectionEnableWait.Start();
        }

        /// <summary>
        /// フォルダ選択ダイアログ表示コマンド実行処理
        /// </summary>
        private void ExecuteCommandShowOpenFolderDialog()
        {
            using CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true,
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FileInspectorData.InspectionDir = dialog.FileName;
            }
        }

        /// <summary>
        /// 検査完了後操作有効待ちタイマのタイムアウト処理
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void TimeoutInspectionEnableWait(object sender, EventArgs e)
        {
            _timerInspectionEnableWait.Stop();

            InspectionState = Properties.Resources.Inspection;

            IsEnableInspection = true;
        }
    }
}
