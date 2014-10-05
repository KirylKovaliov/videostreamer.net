using VideoStreamer.Net.Configuration;

namespace VideoStreamer.Net.Storage
{
    public interface IStorage
    {
        long GetLength(string file);
        byte[] Read(string file, long start, long length);
        void SetupConfig(StorageTypeElement config);
        void ValidateConfig(StorageTypeElement config);
    }
}
