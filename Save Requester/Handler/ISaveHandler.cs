using SaveSystem.SaveService;

namespace SaveSystem.SaveRequester.Handler
{
    public interface ISaveHandler
    {
        bool AcceptSaveRequest(ISaveService saveService, string path);
        bool AcceptLoadRequest(ISaveService saveService, string path);
        bool AcceptDeleteRequest(ISaveService saveService, string path);
    }
}