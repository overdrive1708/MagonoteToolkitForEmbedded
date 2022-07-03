using Prism.Mvvm;

namespace MagonoteToolkitForEmbedded.Models
{
    /// <summary>
    /// 進捗状況モデル
    /// </summary>
    public class ProgressInfo : BindableBase
    {
        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// 最小値
        /// </summary>
        private int _minimum;
        public int Minimum
        {
            get { return _minimum; }
            set { SetProperty(ref _minimum, value); }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        private int _maximum;
        public int Maximum
        {
            get { return _maximum; }
            set { SetProperty(ref _maximum, value); }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        private int _now;
        public int Now
        {
            get { return _now; }
            set { SetProperty(ref _now, value); }
        }

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProgressInfo()
        {
            Minimum = 0;
            Maximum = 0;
            Now = 0;
        }

        /// <summary>
        /// クリア処理
        /// </summary>
        public void Clear()
        {
            Now = Minimum;
        }

        /// <summary>
        /// カウントアップ処理
        /// </summary>
        public void CountUp()
        {
            if (Now < Maximum)
            {
                Now++;
            }
        }
    }
}
