using SaveSystem.SaveService;
using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    [CreateAssetMenu(fileName = RELATIVE_PATH_PROVIDER_NAME, menuName = RELATIVE_PATH_PROVIDER_PATH + RELATIVE_PATH_PROVIDER_NAME)]
    public class RelativeSavePathProviderObject : ScriptableObject, ISavePathProvider
    {
        private const string RELATIVE_PATH_PROVIDER_NAME = "Relative Path Provider";
        private const string RELATIVE_PATH_PROVIDER_PATH = "Save System/";

        [SerializeField]
        [TextArea] private string _relativePath;

        [SerializeField] private string _fileName;

        public ISaveService SaveService { get; set; }

        public string GetSavePath() => new FileSavePathProvider(new SubfolderSavePathProvider(new PersistentDataPathProvider(), _relativePath),
                                                                _fileName,
                                                                SaveService.PreferredFileExtension).GetSavePath();
    }
}