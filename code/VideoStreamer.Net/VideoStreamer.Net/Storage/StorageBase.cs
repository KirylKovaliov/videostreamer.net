using VideoStreamer.Net.Configuration;

namespace VideoStreamer.Net.Storage
{
    /// <summary>
    /// Represets abstract storage
    /// </summary>
    public abstract class StorageBase : IStorage
    {
        protected StorageTypeElement Config { get; private set; }

        public abstract void ValidateConfig(StorageTypeElement config);
        public abstract long GetLength(string file);
        public abstract byte[] Read(string file, long start, long length);

        public void SetupConfig(StorageTypeElement config)
        {
            Config = config;
        }
    }
}
