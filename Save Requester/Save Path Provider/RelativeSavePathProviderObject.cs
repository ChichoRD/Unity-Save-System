using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    [CreateAssetMenu(fileName = RELATIVE_PATH_PROVIDER_NAME, menuName = RELATIVE_PATH_PROVIDER_PATH + RELATIVE_PATH_PROVIDER_NAME)]
    internal class RelativeSavePathProviderObject : ScriptableObject, ISavePathProvider
    {
        private const string RELATIVE_PATH_PROVIDER_NAME = "Relative Path Provider";
        private const string RELATIVE_PATH_PROVIDER_PATH = "Save System/";

        [SerializeField]
        [TextArea] private string _relativePath;

        [SerializeField] private string _fileName;
        [SerializeField] private string _fileExtension;

        public string GetSavePath() => new FileSavePathProvider(new SubfolderSavePathProvider(new PersistentDataPathProvider(), _relativePath),
                                                                _fileName,
                                                                _fileExtension).GetSavePath();
    }
}