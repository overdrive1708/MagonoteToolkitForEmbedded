using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            /// C言語define比較モード
            /// </summary>
            ClangDefine
        }

        /// <summary>
        /// 変更定義情報
        /// </summary>
        public class ChangeInfo
        {
            /// <summary>
            /// 定義名
            /// </summary>
            public string Define { get; set; } = string.Empty;

            /// <summary>
            /// 変更前定義値
            /// </summary>
            public string BeforeValue { get; set; } = string.Empty;

            /// <summary>
            /// 変更後定義値
            /// </summary>
            public string AfterValue { get; set; } = string.Empty;
        }

        /// <summary>
        /// 定義情報
        /// </summary>
        public class DefineInfo
        {
            /// <summary>
            /// 定義名
            /// </summary>
            public string Define { get; set; } = string.Empty;

            /// <summary>
            /// 定義値
            /// </summary>
            public string Value { get; set; } = string.Empty;
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
        private ObservableCollection<ChangeInfo> _changeDefines = new();
        public ObservableCollection<ChangeInfo> ChangeDefines
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
        private List<DefineInfo> _beforeDefineList = new();

        /// <summary>
        /// 変更後定義リスト
        /// </summary>
        private List<DefineInfo> _afterDefineList = new();

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
            ChangeDefines.Clear();

            CreateBeforeDefineList(compareMode);
            CreateAfterDefineList(compareMode);

            ProgressInfoData.Maximum = _beforeDefineList.Count + _afterDefineList.Count;
            if (compareMode == CompareMode.ClangDefine)
            {
                ProgressInfoData.Maximum = ProgressInfoData.Maximum + _afterDefineList.Count;
            }

            CheckAddDefine();
            CheckDeleteDefine();
            if(compareMode == CompareMode.ClangDefine)
            {
                CheckChangeDefine();
            }
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
                DefineInfo addlist = new();

                if (compareMode == CompareMode.ClangDefine)
                {
                    Match match = Regex.Match(define, @"#define[\t ]*(?<definename>[^\t ]*)[\t ]*(?<definevalue>[^\t /]*)");
                    if (match.Success)
                    {
                        addlist.Define = match.Result("${definename}");
                        addlist.Value = match.Result("${definevalue}");
                    }
                }
                else
                {
                    addlist.Define = Regex.Replace(define, "[^a-zA-Z0-9_]", string.Empty);
                    addlist.Value = string.Empty;
                }

                if (!_beforeDefineList.Any(item => item.Define == addlist.Define && item.Value == addlist.Value))
                {
                    _beforeDefineList.Add(addlist);
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
                DefineInfo addlist = new();

                if (compareMode == CompareMode.ClangDefine)
                {
                    Match match = Regex.Match(define, @"#define[\t ]*(?<definename>[^\t ]*)[\t ]*(?<definevalue>[^\t /]*)");
                    if (match.Success)
                    {
                        addlist.Define = match.Result("${definename}");
                        addlist.Value = match.Result("${definevalue}");
                    }
                }
                else
                {
                    addlist.Define = Regex.Replace(define, "[^a-zA-Z0-9_]", string.Empty);
                    addlist.Value = string.Empty;
                }

                if (!_afterDefineList.Any(item => item.Define == addlist.Define && item.Value == addlist.Value))
                {
                    _afterDefineList.Add(addlist);
                }
            }
        }

        /// <summary>
        /// 追加定義のチェック処理
        /// </summary>
        private void CheckAddDefine()
        {
            foreach (DefineInfo defineinfo in _afterDefineList)
            {
                if (!_beforeDefineList.Any(item => item.Define == defineinfo.Define))
                {
                    AddDefines = $"{AddDefines}{defineinfo.Define}\r\n";
                }
                ProgressInfoData.CountUp();
            }
        }

        /// <summary>
        /// 削除定義のチェック処理
        /// </summary>
        private void CheckDeleteDefine()
        {
            foreach (DefineInfo defineinfo in _beforeDefineList)
            {
                if (!_afterDefineList.Any(item => item.Define == defineinfo.Define))
                {
                    DeleteDefines = $"{DeleteDefines}{defineinfo.Define}\r\n";
                }
                ProgressInfoData.CountUp();
            }
        }

        /// <summary>
        /// 変更定義のチェック処理
        /// </summary>
        private void CheckChangeDefine()
        {
            foreach (DefineInfo afterDefineInfo in _afterDefineList)
            {
                DefineInfo beforeDefineInfo = _beforeDefineList.FirstOrDefault(item => item.Define == afterDefineInfo.Define && item.Value != afterDefineInfo.Value);
                if (beforeDefineInfo != null)
                {
                    ChangeInfo changeinfo = new()
                    {
                        Define = afterDefineInfo.Define,
                        BeforeValue = beforeDefineInfo.Value,
                        AfterValue = afterDefineInfo.Value
                    };
                    ChangeDefines.Add(changeinfo);
                }
                ProgressInfoData.CountUp();
            }
        }
    }
}
