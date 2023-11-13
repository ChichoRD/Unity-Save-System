namespace SaveSystem.SaveService
{
    public interface ISaveService
    {
        string PreferredFileExtension { get; }
        public bool Save<T>(T data, string path);
        public bool Load<T>(out T data, string path);
        public bool Delete(string path);
    }
}