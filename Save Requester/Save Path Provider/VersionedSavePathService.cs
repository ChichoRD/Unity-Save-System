using System.IO;
using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    [CreateAssetMenu(fileName = "VersionedSavePathService", menuName = "Save System/Save Path Service/Versioned Save Path Service")]
    internal class VersionedSavePathService : SavePathFlyweight, ISavePathService
    {
        [SerializeField]
        private SavePathFlyweight _savePathFlyweight;
        [SerializeField]
        private bool _placeInVersionedFolder = true;

        public override string GetPath()
        {
            string path = _savePathFlyweight.GetPath();

            string directory = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileExtension = Path.GetExtension(path);

            return _placeInVersionedFolder
                ? Path.Combine(directory, Application.version, Path.ChangeExtension(fileName, fileExtension))
                : Path.Combine(directory, Path.ChangeExtension($"{fileName}{Application.version}", fileExtension));
        }
    }
}