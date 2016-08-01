using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Configurations
{
    public class ServiceElement: ConfigurationElement
    {
        [ConfigurationProperty("serviceIdentifier", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceIdentifier
        {
            get { return ((string)(this["serviceIdentifier"])); }
            set { this["serviceIdentifier"] = value; }
        }

        [ConfigurationProperty("serviceType", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ServiceType
        {
            get { return ((string)(this["serviceType"])); }
            set { this["serviceType"] = value; }
        }

        [ConfigurationProperty("xmlPath", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string XmlPath
        {
            get { return ((string)(this["xmlPath"])); }
            set { this["xmlPath"] = value; }
        }
    }
}
