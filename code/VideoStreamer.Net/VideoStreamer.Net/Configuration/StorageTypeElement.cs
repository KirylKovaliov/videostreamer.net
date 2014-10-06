using System.Configuration;

namespace VideoStreamer.Net.Configuration
{
    public class StorageTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("prefix", IsRequired = true, DefaultValue = "/video")]
        public string Prefix
        {
            get { return this["prefix"].ToString().ToLower(); }
            set { this["prefix"] = value.ToLower(); }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type 
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("folder", IsRequired = false)]
        public string Folder
        {
            get { return (string)this["folder"]; }
            set { this["folder"] = value; }
        }

        [ConfigurationProperty("accessKeyId", IsRequired = false)]
        public string AccessKeyId
        {
            get { return (string)this["accessKeyId"]; }
            set { this["AccessKeyId"] = value; }
        }

        [ConfigurationProperty("secretAccessKey", IsRequired = false)]
        public string SecretAccessKey
        {
            get { return (string)this["secretAccessKey"]; }
            set { this["secretAccessKey"] = value; }
        }

        [ConfigurationProperty("region", IsRequired = false)]
        public string Region
        {
            get { return (string)this["region"]; }
            set { this["region"] = value; }
        }

        [ConfigurationProperty("bucket", IsRequired = false)]
        public string Bucket
        {
            get { return (string)this["bucket"]; }
            set { this["bucket"] = value; }
        }
    }
}
