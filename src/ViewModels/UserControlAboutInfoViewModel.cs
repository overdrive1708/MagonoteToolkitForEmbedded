using System.Reflection;
using System.Diagnostics;
using Prism.Mvvm;
using Prism.Commands;

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
        private string _productBody = string.Empty;
        public string ProductBody
        {
            get { return _productBody; }
            set { SetProperty(ref _productBody, value); }
        }

        /// <summary>
        /// バージョン
        /// </summary>
        private string _versionBody = string.Empty;
        public string VersionBody
        {
            get { return _versionBody; }
            set { SetProperty(ref _versionBody, value); }
        }

        /// <summary>
        /// ライセンス
        /// </summary>
        private string _licenseBody = string.Empty;
        public string LicenseBody
        {
            get { return _licenseBody; }
            set { SetProperty(ref _licenseBody, value); }
        }

        //--------------------------------------------------
        // バインディングコマンド(スニペット:cmd/cmdg)
        //--------------------------------------------------
        /// <summary>
        /// LicenseURLを開く
        /// </summary>
        private DelegateCommand<string> _commandOpenLicenseUrl;
        public DelegateCommand<string> CommandTransitionView =>
            _commandOpenLicenseUrl ?? (_commandOpenLicenseUrl = new DelegateCommand<string>(ExecuteCommandOpenLicenseUrl));

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
                ProductBody = assm.GetCustomAttribute<AssemblyProductAttribute>().Product;

                VersionBody = assm.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

                LicenseBody = $"{assm.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright}\r\n{Properties.Resources.AboutInfoLicenseBody}";
            }
        }

        /// <summary>
        /// LicenseURLを開くコマンド実行処理
        /// </summary>
        private void ExecuteCommandOpenLicenseUrl(string url)
        {
            ProcessStartInfo psi = new()
            {
                FileName = url,
                UseShellExecute = true,
            };
            Process.Start(psi);
        }
    }
}
