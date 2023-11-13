using SaveSystem.SaveService;

namespace SaveSystem.SaveRequester
{
    public interface ISaveRequester
    {
        bool Initialized { get; }
        bool Initialize(ISaveService saveService);
        bool Save(string path);
        bool Load(string path);
        bool Delete(string path);
    }
}