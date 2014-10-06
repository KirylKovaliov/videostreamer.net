using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace VideoStreamer.Net.Configuration
{
    [ConfigurationCollection(typeof(StorageTypeElement), AddItemName = "storage")]
    public class StorageTypeElementCollection : ConfigurationElementCollection, IEnumerable<StorageTypeElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StorageTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as StorageTypeElement;
            return configElement != null ? configElement.Type : string.Empty;
        }

        public StorageTypeElement this[int index]
        {
            get { return BaseGet(index) as StorageTypeElement; }
        }

        IEnumerator<StorageTypeElement> IEnumerable<StorageTypeElement>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }
    }
}
