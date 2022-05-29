using Prism.Mvvm;
using System.Reflection;

namespace MagonoteToolkitForEmbedded.ViewModels
{
    public class UserControlAboutInfoViewModel : BindableBase
    {
        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// 製品名
        /// </summary>
        private string _product = "製品名:";
        public string Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }

        /// <summary>
        /// バージョン
        /// </summary>
        private string _version = "バージョン:";
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        /// <summary>
        /// 著作権
        /// </summary>
        private string _copyright = "著作権:";
        public string Copyright
        {
            get { return _copyright; }
            set { SetProperty(ref _copyright, value); }
        }

        //--------------------------------------------------
        // 変数
        //--------------------------------------------------
        static public string title = "バージョン情報";

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlAboutInfoViewModel()
        {
            // アセンブリ情報を取得して表示値を更新
            Assembly assm = Assembly.GetExecutingAssembly();
            if (assm != null)
            {
                Product += assm.GetCustomAttribute<AssemblyProductAttribute>().Product;

                Version += assm.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

                Copyright += assm.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            }
        }
    }
}
