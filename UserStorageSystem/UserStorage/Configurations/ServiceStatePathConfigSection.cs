using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Configurations
{
    public class ServiceStatePathConfigSection: ConfigurationSection
    {
        [ConfigurationProperty("ServiceStatePathes")]
        public ServiceStatePathCollection ServiceStatePathItems
        {
            get { return ((ServiceStatePathCollection)(base["ServiceStatePathes"])); }
        }
    }
}
