using VideoStreamer.Net.Configuration;

namespace VideoStreamer.Net.Storage
{
    /// <summary>
    /// Represets abstract storage
    /// </summary>
    public abstract class StorageBase : IStorage
    {
        public StorageTypeElement Config { get; set; }

        public abstract void ValidateConfig(StorageTypeElement config);
        public abstract long GetLength(string file);
        public abstract byte[] Read(string file, long start, long length);
    }
}
