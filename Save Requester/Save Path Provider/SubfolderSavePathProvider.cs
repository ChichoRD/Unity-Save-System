using System.IO;

namespace SaveSystem.SaveRequester.SavePath
{
    internal readonly struct SubfolderSavePathProvider : ISavePathProvider
    {
        private readonly string _subfolderName;

        public SubfolderSavePathProvider(ISavePathProvider pathProvider, string subfolderName)
        {
            _subfolderName = Path.Combine(pathProvider.GetSavePath(), subfolderName);
        }

        public readonly string GetSavePath() => _subfolderName;
    }
}
