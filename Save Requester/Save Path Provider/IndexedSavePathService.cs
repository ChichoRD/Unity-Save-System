using System.IO;
using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    [CreateAssetMenu(fileName = "IndexedSavePathService", menuName = "Save System/Save Path Service/Indexed Save Path Service")]
    internal class IndexedSavePathService : SavePathFlyweight, ISavePathService
    {
        [SerializeField]
        private SavePathFlyweight _savePathFlyweight;
        [SerializeField]
        private int _index;
        [SerializeField]
        private bool _placeInIndexedFolder = true;

        public override string GetPath()
        {
            string path = _savePathFlyweight.GetPath();

            string directory = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileExtension = Path.GetExtension(path);

            return _placeInIndexedFolder
                ? Path.Combine(directory, _index.ToString(), Path.ChangeExtension(fileName, fileExtension))
                : Path.Combine(directory, Path.ChangeExtension($"{fileName}{_index}", fileExtension));
        }
    }
}