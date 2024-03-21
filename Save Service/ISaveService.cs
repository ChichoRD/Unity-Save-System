namespace SaveSystem.SaveService
{
    public interface ISaveService
    {
        public bool Save<T>(T data, string path);
        public bool Load<T>(out T data, string path);
        public bool Delete(string path);
    }
}