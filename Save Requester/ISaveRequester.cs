using SaveSystem.SaveRequester.Handler;

namespace SaveSystem.SaveRequester
{
    public interface ISaveRequester
    {
        bool Save(ISaveHandler saveHandler, string path);
        bool Load(ISaveHandler saveHandler, string path);
        bool Delete(ISaveHandler saveHandler, string path);
    }
}