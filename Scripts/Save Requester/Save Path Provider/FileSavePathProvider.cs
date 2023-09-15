using System.IO;

public readonly struct FileSavePathProvider : ISavePathProvider
{
    private readonly string _filePath;

    public FileSavePathProvider(ISavePathProvider pathProvider, string fileName, string fileExtension)
    {
        _filePath = Path.Combine(pathProvider.GetSavePath(), $"{fileName}{fileExtension}");
    }

    public FileSavePathProvider(ISavePathProvider pathProvider, string fileName, ISaveService saveService)
    {
        _filePath = Path.Combine(pathProvider.GetSavePath(), $"{fileName}{saveService.PreferredFileExtension}");
    }

    public readonly string GetSavePath() => _filePath;
}
