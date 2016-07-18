using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Configurations
{
    public class ServiceStatePathElement: ConfigurationElement
    {
        [ConfigurationProperty("serviceIdentifier", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceIdentifier
        {
            get { return ((string)(base["serviceIdentifier"])); }
            set { base["serviceIdentifier"] = value; }
        }

        [ConfigurationProperty("serviceStatePath", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string ServiceStatePath
        {
            get { return ((string)(base["serviceStatePath"])); }
            set { base["serviceStatePath"] = value; }
        }
    }
}
