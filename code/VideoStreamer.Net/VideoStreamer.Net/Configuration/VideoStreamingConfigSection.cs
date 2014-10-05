using System.Configuration;

namespace VideoStreamer.Net.Configuration
{
    public class VideoStreamingConfigSection : ConfigurationSection 
    {
        [ConfigurationProperty("storage")]
        public StorageTypeElement Storage
        {
            get { return (StorageTypeElement) this["storage"]; }
            set { this["storage"] = value; }
        }

        [ConfigurationProperty("prefix", IsRequired = true, DefaultValue = "/video")]
        public string Prefix
        {
            get { return this["prefix"].ToString(); }
            set { this["prefix"] = value; }
        }
    }
}
