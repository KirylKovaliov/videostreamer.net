using System.Configuration;

namespace VideoStreamer.Net.Configuration
{
    public class VideoStreamingConfigSection : ConfigurationSection 
    {
        [ConfigurationProperty("storages", IsRequired = true)]
        public StorageTypeElementCollection Storages
        {
            get { return (StorageTypeElementCollection)this["storages"]; }
            set { this["storages"] = value; }
        }
    }
}
