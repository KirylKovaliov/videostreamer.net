using VideoStreamer.Net.Configuration;

namespace VideoStreamer.Net.Storage
{
    public interface IStorage
    {
        StorageTypeElement Config { get; set; }
        long GetLength(string file);
        byte[] Read(string file, long start, long end);
        void ValidateConfig(StorageTypeElement config);
    }
}
