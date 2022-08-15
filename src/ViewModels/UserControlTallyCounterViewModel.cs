using MagonoteToolkitForEmbedded.Models;
using MagonoteToolkitForEmbedded.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Input;

namespace MagonoteToolkitForEmbedded.ViewModels
{
    public class UserControlTallyCounterViewModel : BindableBase, INavigationAware
    {
        //--------------------------------------------------
        // 公開定義
        //--------------------------------------------------
        /// <summary>
        /// カウンタ番号
        /// </summary>
        public enum CounterNum
        {
            Counter1,
            Counter2,
            Counter3,
            Counter4,
            Counter5,
            CounterMax
        }

        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// 選択中のカウンタ番号
        /// </summary>
        private CounterNum _selectedCounterNum = CounterNum.Counter1;
        public CounterNum SelectedCounterNum
        {
            get { return _selectedCounterNum; }
            set { SetProperty(ref _selectedCounterNum, value); }
        }

        /// <summary>
        /// カウント値文字列(No.1)
        /// </summary>
        private string _countString1 = 0.ToString();
        public string CountString1
        {
            get { return _countString1; }
            set { SetProperty(ref _countString1, value); }
        }

        /// <summary>
        /// カウント値文字列(No.2)
        /// </summary>
        private string _countString2 = 0.ToString();
        public string CountString2
        {
            get { return _countString2; }
            set { SetProperty(ref _countString2, value); }
        }

        /// <summary>
        /// カウント値文字列(No.3)
        /// </summary>
        private string _countString3 = 0.ToString();
        public string CountString3
        {
            get { return _countString3; }
            set { SetProperty(ref _countString3, value); }
        }

        /// <summary>
        /// カウント値文字列(No.4)
        /// </summary>
        private string _countString4 = 0.ToString();
        public string CountString4
        {
            get { return _countString4; }
            set { SetProperty(ref _countString4, value); }
        }

        /// <summary>
        /// カウント値文字列(No.5)
        /// </summary>
        private string _countString5 = 0.ToString();
        public string CountString5
        {
            get { return _countString5; }
            set { SetProperty(ref _countString5, value); }
        }

        //--------------------------------------------------
        // バインディングコマンド(スニペット:cmd/cmdg)
        //--------------------------------------------------
        /// <summary>
        /// カウントアップコマンド
        /// </summary>
        private DelegateCommand<string> _commandCountUp;
        public DelegateCommand<string> CommandCountUp =>
            _commandCountUp ?? (_commandCountUp = new DelegateCommand<string>(ExecuteCommandCountUp));

        /// <summary>
        /// カウントダウンコマンド
        /// </summary>
        private DelegateCommand<string> _commandCountDown;
        public DelegateCommand<string> CommandCountDown =>
            _commandCountDown ?? (_commandCountDown = new DelegateCommand<string>(ExecuteCommandCountDown));

        /// <summary>
        /// カウントクリアコマンド
        /// </summary>
        private DelegateCommand<string> _commandCountClear;
        public DelegateCommand<string> CommandCountClear =>
            _commandCountClear ?? (_commandCountClear = new DelegateCommand<string>(ExecuteCommandCountClear));

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// EventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// カウンタ
        /// </summary>
        private readonly Counter[] _counter = new Counter[(int)CounterNum.CounterMax];

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ(XAMLデザイナー用)
        /// </summary>
        public UserControlTallyCounterViewModel()
        {
            // 無処理
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventAggregator">EventAggregator</param>
        public UserControlTallyCounterViewModel(IEventAggregator eventAggregator)
        {
            // 内部変数の初期化
            for (CounterNum loopcnt = CounterNum.Counter1; loopcnt < CounterNum.CounterMax; loopcnt++)
            {
                _counter[(int)loopcnt] = new Counter();
            }

            // EventAggregatorを保存する
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// ナビゲーション対象にしていい状態かどうか
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns>インスタンスを再利用するためtrue固定</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <summary>
        /// 画面表示開始処理
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // キー入力イベントの購読
            _eventAggregator.GetEvent<KeyInputEvent>().Subscribe(ReceiveKeyInputEvent);
        }

        /// <summary>
        /// 画面表示終了処理
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // キー入力イベントの購読解除
            _eventAggregator.GetEvent<KeyInputEvent>().Unsubscribe(ReceiveKeyInputEvent);
        }

        /// <summary>
        /// カウントアップコマンド実行処理
        /// </summary>
        /// <param name="targetCounterNum">操作対象のカウンタ番号</param>
        private void ExecuteCommandCountUp(string targetCounterNum)
        {
            ChangeCounter(targetCounterNum);
            CountUpSelectedCounter();
        }

        /// <summary>
        /// カウントクリアコマンド実行処理
        /// </summary>
        /// <param name="targetCounterNum">操作対象のカウンタ番号</param>
        private void ExecuteCommandCountClear(string targetCounterNum)
        {
            ChangeCounter(targetCounterNum);
            CountClearSelectedCounter();
            
        }

        /// <summary>
        /// カウントダウンコマンド実行処理
        /// </summary>
        /// <param name="targetCounterNum">操作対象のカウンタ番号</param>
        private void ExecuteCommandCountDown(string targetCounterNum)
        {
            ChangeCounter(targetCounterNum);
            CountDownSelectedCounter();
        }

        /// <summary>
        /// 選択中カウンタのカウントアップ処理
        /// </summary>
        private void CountUpSelectedCounter()
        {
            _counter[(int)SelectedCounterNum].CountUp();
            UpdateCountString();
        }

        /// <summary>
        /// 選択中カウンタのカウントダウン処理
        /// </summary>
        private void CountDownSelectedCounter()
        {
            _counter[(int)SelectedCounterNum].CountDown();
            UpdateCountString();
        }

        /// <summary>
        /// 選択中カウンタのカウントクリア処理
        /// </summary>
        private void CountClearSelectedCounter()
        {
            _counter[(int)SelectedCounterNum].Clear();
            UpdateCountString();
        }

        /// <summary>
        /// 選択中カウンタのカウント値文字列更新処理
        /// </summary>
        private void UpdateCountString()
        {
            string countString = _counter[(int)SelectedCounterNum].CountValue.ToString();

            switch (SelectedCounterNum)
            {
                case CounterNum.Counter1:
                    CountString1 = countString;
                    break;
                case CounterNum.Counter2:
                    CountString2 = countString;
                    break;
                case CounterNum.Counter3:
                    CountString3 = countString;
                    break;
                case CounterNum.Counter4:
                    CountString4 = countString;
                    break;
                case CounterNum.Counter5:
                    CountString5 = countString;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// キー入力イベント受信処理
        /// </summary>
        /// <param name="inputKey">入力されたキー</param>
        private void ReceiveKeyInputEvent(Key inputKey)
        {
            // 入力されたキーに応じて処理を行う
            switch (inputKey)
            {
                case Key.NumPad0:
                    CountClearSelectedCounter();
                    break;
                case Key.NumPad1:
                    ChangeCounter("Counter1");
                    break;
                case Key.NumPad2:
                    ChangeCounter("Counter2");
                    break;
                case Key.NumPad3:
                    ChangeCounter("Counter3");
                    break;
                case Key.NumPad4:
                    ChangeCounter("Counter4");
                    break;
                case Key.NumPad5:
                    ChangeCounter("Counter5");
                    break;
                case Key.D1:
                    ChangeCounter("Counter1");
                    break;
                case Key.D2:
                    ChangeCounter("Counter2");
                    break;
                case Key.D3:
                    ChangeCounter("Counter3");
                    break;
                case Key.D4:
                    ChangeCounter("Counter4");
                    break;
                case Key.D5:
                    ChangeCounter("Counter5");
                    break;
                case Key.S:
                    CountDownSelectedCounter();
                    break;
                case Key.W:
                    CountUpSelectedCounter();
                    break;
                case Key.Back:
                    CountClearSelectedCounter();
                    break;
                case Key.Add:
                    CountUpSelectedCounter();
                    break;
                case Key.Subtract:
                    CountDownSelectedCounter();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// カウンタ変更処理(番号指定)
        /// </summary>
        /// <param name="targetCounterNum">変更対象のカウンタ番号(Counter1～Counter5)</param>
        private void ChangeCounter(string targetCounterNum)
        {
            SelectedCounterNum = targetCounterNum switch
            {
                "Counter1" => CounterNum.Counter1,
                "Counter2" => CounterNum.Counter2,
                "Counter3" => CounterNum.Counter3,
                "Counter4" => CounterNum.Counter4,
                "Counter5" => CounterNum.Counter5,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
