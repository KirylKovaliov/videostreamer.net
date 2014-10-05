using System.Collections.Specialized;
using System.Configuration;

namespace VideoStreamer.Net.Configuration
{
    public class StorageTypeElement : ConfigurationElement
    {
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


        public NameValueCollection Parameters
        {
            get
            {
                //TODO: read the rest attributes
                return new NameValueCollection();
            }
        }
    }
}
