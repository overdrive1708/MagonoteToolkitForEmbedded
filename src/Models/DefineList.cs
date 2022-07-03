using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Prism.Mvvm;

namespace MagonoteToolkitForEmbedded.Models
{
    /// <summary>
    /// 定義リストモデル
    /// </summary>
    public class DefineList : BindableBase
    {
        //--------------------------------------------------
        // 公開定義
        //--------------------------------------------------
        /// <summary>
        /// 比較モード
        /// </summary>
        public enum CompareMode
        {
            /// <summary>
            /// 一般モード
            /// </summary>
            General,
            /// <summary>
            /// C言語モード
            /// </summary>
            C
        }

        //--------------------------------------------------
        // バインディングデータ(スニペット:propp)
        //--------------------------------------------------
        /// <summary>
        /// 進捗状況データ
        /// </summary>
        private ProgressInfo _progressInfoData = new();
        public ProgressInfo ProgressInfoData
        {
            get { return _progressInfoData; }
            set { SetProperty(ref _progressInfoData, value); }
        }

        /// <summary>
        /// 変更前定義
        /// </summary>
        private string _beforeDefines = string.Empty;
        public string BeforeDefines
        {
            get { return _beforeDefines; }
            set { SetProperty(ref _beforeDefines, value); }
        }

        /// <summary>
        /// 変更後定義
        /// </summary>
        private string _afterDefines = string.Empty;
        public string AfterDefines
        {
            get { return _afterDefines; }
            set { SetProperty(ref _afterDefines, value); }
        }

        /// <summary>
        /// 追加定義
        /// </summary>
        private string _addDefines = string.Empty;
        public string AddDefines
        {
            get { return _addDefines; }
            set { SetProperty(ref _addDefines, value); }
        }

        /// <summary>
        /// 削除定義
        /// </summary>
        private string _deleteDefines = string.Empty;
        public string DeleteDefines
        {
            get { return _deleteDefines; }
            set { SetProperty(ref _deleteDefines, value); }
        }

        /// <summary>
        /// 変更定義
        /// </summary>
        private string _changeDefines = string.Empty;
        public string ChangeDefines
        {
            get { return _changeDefines; }
            set { SetProperty(ref _changeDefines, value); }
        }

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 変更前定義リスト
        /// </summary>
        private List<string> _beforeDefineList = new();

        /// <summary>
        /// 変更後定義リスト
        /// </summary>
        private List<string> _afterDefineList = new();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DefineList()
        {
            // 無処理
        }

        /// <summary>
        /// 比較処理
        /// </summary>
        /// <param name="compareMode">比較モード</param>
        public void Compare(CompareMode compareMode)
        {
            ProgressInfoData.Clear();

            AddDefines = string.Empty;
            DeleteDefines = string.Empty;

            CreateBeforeDefineList(compareMode);
            CreateAfterDefineList(compareMode);

            ProgressInfoData.Maximum = _beforeDefineList.Count + _afterDefineList.Count;

            CheckAddDefine();
            CheckDeleteDefine();
            // TODO:C言語モードの場合は定義値の比較を行う｡C言語モードを実装するときは､定義リストは値を考慮したクラスに変更する｡
        }

        /// <summary>
        /// 変更前定義リスト生成処理
        /// </summary>
        /// /// <param name="compareMode">比較モード</param>
        private void CreateBeforeDefineList(CompareMode compareMode)
        {
            _beforeDefineList.Clear();

            string[] defines = BeforeDefines.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string define in defines)
            {
                string addstr = define;

                if (compareMode == CompareMode.General)
                {
                    addstr = Regex.Replace(addstr, "[^a-zA-Z0-9_]", String.Empty);
                }

                if (!_beforeDefineList.Contains(addstr))
                {
                    _beforeDefineList.Add(addstr);
                }
            }
        }

        /// <summary>
        /// 変更後定義リスト生成処理
        /// </summary>
        /// <param name="compareMode">比較モード</param>
        private void CreateAfterDefineList(CompareMode compareMode)
        {
            _afterDefineList.Clear();

            string[] defines = AfterDefines.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string define in defines)
            {
                string addstr = define;

                if (compareMode == CompareMode.General)
                {
                    addstr = Regex.Replace(addstr, "[^a-zA-Z0-9_]", string.Empty);
                }

                if (!_afterDefineList.Contains(addstr))
                {
                    _afterDefineList.Add(addstr);
                }
            }
        }

        /// <summary>
        /// 追加定義のチェック処理
        /// </summary>
        private void CheckAddDefine()
        {
            foreach (string define in _afterDefineList)
            {
                if (!_beforeDefineList.Contains(define))
                {
                    AddDefines = $"{AddDefines}{define}\r\n";
                }
                ProgressInfoData.CountUp();
            }
        }

        /// <summary>
        /// 削除定義のチェック処理
        /// </summary>
        private void CheckDeleteDefine()
        {
            foreach (string define in _beforeDefineList)
            {
                if (!_afterDefineList.Contains(define))
                {
                    DeleteDefines = $"{DeleteDefines}{define}\r\n";
                }
                ProgressInfoData.CountUp();
            }
        }
    }
}
