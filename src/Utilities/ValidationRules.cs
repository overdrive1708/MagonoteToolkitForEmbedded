using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace MagonoteToolkitForEmbedded.Utilities
{
    /// <summary>
    /// バリデーションルール(空白かどうか)
    /// </summary>
    internal class NotEmptyValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, Message ?? Properties.Resources.MessageFieldIsRequired);
            }
            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// バリデーションルール(フォルダが存在するかどうか)
    /// </summary>
    internal class ExistFolderValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!Directory.Exists(value?.ToString()))
            {
                return new ValidationResult(false, Message ?? Properties.Resources.MessageFolderNotFound);
            }
            return ValidationResult.ValidResult;
        }
    }
}
