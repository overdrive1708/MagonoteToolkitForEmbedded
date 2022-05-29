using Prism.Mvvm;

namespace MagonoteToolkitForEmbedded.ViewModels
{
    public class UserControlWelcomeViewModel : BindableBase
    {
        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// ウェルカムメッセージ(タイトル)
        /// </summary>
        private string _welcomeMessageTitle = "｢Magonote Toolkit For Embedded｣へようこそ!";
        public string WelcomeMessageTitle
        {
            get { return _welcomeMessageTitle; }
            set { SetProperty(ref _welcomeMessageTitle, value); }
        }

        /// <summary>
        /// ウェルカムメッセージ(本文)
        /// </summary>
        private string _welcomeMessageBody = 
            "このツールは組み込みソフトウェア開発に便利なツールを集めたツールキットです｡\r\n" +
            "メニューバーからツールを選んでください｡";
        public string WelcomeMessageBody
        {
            get { return _welcomeMessageBody; }
            set { SetProperty(ref _welcomeMessageBody, value); }
        }

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlWelcomeViewModel()
        {
            // 無処理
        }
    }
}
