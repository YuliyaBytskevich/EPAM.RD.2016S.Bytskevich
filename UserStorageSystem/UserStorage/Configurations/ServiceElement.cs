using System.Configuration;

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

        [ConfigurationProperty("host", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Host
        {
            get { return ((string)(this["host"])); }
            set { this["host"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = 1000, IsKey = false, IsRequired = true)]
        public int Port
        {
            get { return ((int)(this["port"])); }
            set { this["port"] = value; }
        }
    }
}
