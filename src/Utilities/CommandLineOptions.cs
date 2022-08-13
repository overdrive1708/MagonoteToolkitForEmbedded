namespace MagonoteToolkitForEmbedded.Utilities
{
    internal class CommandLineOptions
    {
        /// <summary>
        /// カルチャーの切り替え
        /// </summary>
        [CommandLine.Option('c', "cultureinfo", Required = false)]
        public string CultureInfo { get => _cultureInfo; set => _cultureInfo = value; }
        private string _cultureInfo = string.Empty;
    }
}
