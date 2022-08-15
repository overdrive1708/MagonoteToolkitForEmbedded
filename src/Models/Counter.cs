namespace MagonoteToolkitForEmbedded.Models
{
    /// <summary>
    /// カウンタモデル
    /// </summary>
    public class Counter
    {
        //--------------------------------------------------
        // プロパティ
        //--------------------------------------------------
        /// <summary>
        /// カウント値
        /// </summary>
        public uint CountValue { get; set; }

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Counter() => CountValue = 0;

        /// <summary>
        /// カウントアップ処理
        /// </summary>
        public void CountUp()
        {
            if (CountValue < uint.MaxValue)
            {
                CountValue++;
            }
        }

        /// <summary>
        /// カウントダウン処理
        /// </summary>
        public void CountDown()
        {
            if (uint.MinValue < CountValue)
            {
                CountValue--;
            }
        }

        /// <summary>
        /// カウントクリア処理
        /// </summary>
        public void Clear() => CountValue = uint.MinValue;
    }
}
