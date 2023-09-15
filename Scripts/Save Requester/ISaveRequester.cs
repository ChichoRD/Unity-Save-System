public interface ISaveRequester
{
    bool Initialize(ISaveService saveService);
    bool Save(string path);
    bool Load(string path);
    bool Delete(string path);
}
