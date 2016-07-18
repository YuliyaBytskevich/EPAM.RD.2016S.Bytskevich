using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Configurations
{
    [ConfigurationCollection(typeof(ServiceStatePathElement))]
    public class ServiceStatePathCollection: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceStatePathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceStatePathElement)(element)).ServiceIdentifier;
        }

        public ServiceStatePathElement this[int i]
        {
            get { return (ServiceStatePathElement)BaseGet(i); }
        }

    }
}
