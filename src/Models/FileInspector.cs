using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MagonoteToolkitForEmbedded.Models
{
    /// <summary>
    /// ファイル検査モデル
    /// </summary>
    public class FileInspector : BindableBase
    {
        //--------------------------------------------------
        // 公開定義
        //--------------------------------------------------
        /// <summary>
        /// 検査結果
        /// </summary>
        public class InspectionResult
        {
            /// <summary>
            /// ファイル名
            /// </summary>
            public string FileName { get; set; } = string.Empty;

            /// <summary>
            /// 検査結果(ファイル末尾の改行)
            /// </summary>
            public string ResultNewlineAtEOF { get; set; } = Properties.Resources.ResultNA;

            /// <summary>
            /// 検査結果(改行コード)
            /// </summary>
            public string ResultLineFeedCode { get; set; } = Properties.Resources.ResultNA;
            
            /// <summary>
            /// 検査結果にエラーを含むかどうか
            /// </summary>
            public bool HasError { get; set; } = false;

            /// <summary>
            /// 検査結果にエラーを含むかどうかの判定処理
            /// </summary>
            public void CreateHasError()
            {
                if (ResultNewlineAtEOF == Properties.Resources.ResultNG)
                {
                    HasError = true;
                }
                else if (ResultLineFeedCode == Properties.Resources.LineFeedCodeMixed)
                {
                    HasError = true;
                }
                else if (ResultLineFeedCode == Properties.Resources.LineFeedCodeUnknown)
                {
                    HasError = true;
                }
                else
                {
                    HasError = false;
                }
            }
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
        /// 検査対象フォルダ
        /// </summary>
        private string _inspectionDir = string.Empty;
        public string InspectionDir
        {
            get { return _inspectionDir; }
            // ValidationRulesを適用している場合､一度正常値が入力されてしまうと､
            // その後異常値が入力されても､値に変更がない｡
            // その後前回の正常値を入力すると､値に変更がないため､SetPropertyだとViewが更新されない｡
            // そのため､セット時に強制的にViewを更新させる｡
            set { _inspectionDir = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 設定項目(ファイル末尾の改行)
        /// </summary>
        private ObservableCollection<string> _configListNewlineAtEOF = new() { Properties.Resources.CheckDisabled, Properties.Resources.CheckEnabled };
        public ObservableCollection<string> ConfigListNewlineAtEOF
        {
            get { return _configListNewlineAtEOF; }
            set { SetProperty(ref _configListNewlineAtEOF, value); }
        }

        /// <summary>
        /// 設定値(ファイル末尾の改行)
        /// </summary>
        private string _configValueNewlineAtEOF = Properties.Resources.CheckEnabled;
        public string ConfigValueNewlineAtEOF
        {
            get { return _configValueNewlineAtEOF; }
            set { SetProperty(ref _configValueNewlineAtEOF, value); }
        }

        /// <summary>
        /// 設定項目(改行コード)
        /// </summary>
        private ObservableCollection<string> _configListLineFeedCode = new() { Properties.Resources.CheckDisabled, Properties.Resources.CheckEnabled };
        public ObservableCollection<string> ConfigListLineFeedCode
        {
            get { return _configListLineFeedCode; }
            set { SetProperty(ref _configListLineFeedCode, value); }
        }

        /// <summary>
        /// 設定値(改行コード)
        /// </summary>
        private string _configValueLineFeedCode = Properties.Resources.CheckEnabled;
        public string ConfigValueLineFeedCode
        {
            get { return _configValueLineFeedCode; }
            set { SetProperty(ref _configValueLineFeedCode, value); }
        }

        /// <summary>
        /// 検査結果
        /// </summary>
        private ObservableCollection<InspectionResult> _inspectionResultData = new();
        public ObservableCollection<InspectionResult> InspectionResultData
        {
            get { return _inspectionResultData; }
            set { SetProperty(ref _inspectionResultData, value); }
        }

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 検査対象ファイルリスト
        /// </summary>
        private readonly List<string> _inspectionFiles = new();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileInspector()
        {
            // shift_jisをサポートするようにする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 検査処理
        /// </summary>
        public void Inspection()
        {
            ProgressInfoData.Clear();

            InspectionResultData.Clear();
            
            ListupInspectionFiles();

            ProgressInfoData.Maximum = _inspectionFiles.Count;

            foreach (string file in _inspectionFiles)
            {
                InspectionResult inspectionResult = new()
                {
                    FileName = file
                };

                if (ConfigValueNewlineAtEOF == Properties.Resources.CheckEnabled)
                {
                    inspectionResult.ResultNewlineAtEOF = InspectionNewlineAtEOF(file);
                }
                else
                {
                    inspectionResult.ResultNewlineAtEOF = Properties.Resources.ResultNA;
                }
                
                if (ConfigValueLineFeedCode == Properties.Resources.CheckEnabled)
                {
                    inspectionResult.ResultLineFeedCode = InspectionLineFeedCode(file);
                }
                else
                {
                    inspectionResult.ResultLineFeedCode = Properties.Resources.ResultNA;
                }

                inspectionResult.CreateHasError();

                InspectionResultData.Add(inspectionResult);

                ProgressInfoData.CountUp();
            }
        }

        /// <summary>
        /// 検索対象ファイルのリストアップ
        /// </summary>
        private void ListupInspectionFiles()
        {
            _inspectionFiles.Clear();

            IEnumerable<string> files = Directory.EnumerateFiles(InspectionDir, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                _inspectionFiles.Add(file);
            }
        }

        /// <summary>
        /// 検査処理(ファイル末尾の改行)
        /// </summary>
        /// <param name="file">検査対象ファイル</param>
        /// <returns>検査結果</returns>
        private static string InspectionNewlineAtEOF(string file)
        {
            using (FileStream fs = new(file, FileMode.Open, FileAccess.Read))
            {
                int readByte = 0;
                List<byte> readBytes = new();
                string readString = string.Empty;

                // ファイル末尾から2Byte取得してCRLFかどうか判定
                if (fs.Length >= 2)
                {
                    readBytes.Clear();
                    fs.Seek(-2, SeekOrigin.End);
                    while ((readByte = fs.ReadByte()) >= 0)
                    {
                        readBytes.Add((byte)readByte);
                    }
                    readString = Encoding.GetEncoding("shift_jis").GetString(readBytes.ToArray());
                    if (readString.Contains("\r\n"))
                    {
                        return Properties.Resources.ResultOK;
                    }
                }

                // ファイル末尾から1Byte取得してCRもしくはLFかどうか判定
                if (fs.Length >= 1)
                {
                    readBytes.Clear();
                    fs.Seek(-1, SeekOrigin.End);
                    while ((readByte = fs.ReadByte()) >= 0)
                    {
                        readBytes.Add((byte)readByte);
                    }
                    readString = Encoding.GetEncoding("shift_jis").GetString(readBytes.ToArray());
                    if (readString.Contains('\r'))
                    {
                        return Properties.Resources.ResultOK;
                    }
                    if (readString.Contains('\n'))
                    {
                        return Properties.Resources.ResultOK;
                    }
                }
            }
            return Properties.Resources.ResultNG;
        }

        /// <summary>
        /// 検査処理(改行コード)
        /// </summary>
        /// <param name="file">検査対象ファイル</param>
        /// <returns>検査結果</returns>
        private static string InspectionLineFeedCode(string file)
        {
            using StreamReader streamReader = new(file, Encoding.GetEncoding("shift_jis"));
            string alltext = streamReader.ReadToEnd();
            string replacedCrLf = alltext.Replace("\r\n","");
            string replacedCr = alltext.Replace("\r", "");
            string replacedLf = alltext.Replace("\n", "");

            if (alltext.Contains("\r\n"))
            {
                // CR+LFを含む場合､CR+LFを除去し､CR or LFが残っていたら混在｡残っていなかったらCR+LFのみ｡
                if (replacedCrLf.Contains('\r'))
                {
                    return Properties.Resources.LineFeedCodeMixed;
                }
                else if (replacedCrLf.Contains('\n'))
                {
                    return Properties.Resources.LineFeedCodeMixed;
                }
                else
                {
                    return Properties.Resources.LineFeedCodeOnlyCRLF;
                }
            }
            else if (alltext.Contains('\r'))
            {
                // CR+LFがなくCRを含む場合､CRを除去し､LFが残っていたら混在｡残っていなかったらCRのみ｡
                if (replacedCr.Contains('\n'))
                {
                    return Properties.Resources.LineFeedCodeMixed;
                }
                else
                {
                    return Properties.Resources.LineFeedCodeOnlyCR;
                }
            }
            else if (alltext.Contains('\n'))
            {
                // CR+LFがなくCRがなくLFを含む場合､LFのみ｡
                return Properties.Resources.LineFeedCodeOnlyLF;
            }
            else
            {
                // 上記判定に該当しない場合は判定不可｡
                return Properties.Resources.LineFeedCodeUnknown;
            }
        }
    }
}
