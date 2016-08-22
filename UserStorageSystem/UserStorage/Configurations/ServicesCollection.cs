using System.Configuration;

namespace UserStorage.Configurations
{
    [ConfigurationCollection(typeof(ServiceElement))]
    public class ServicesCollection: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)(element)).ServiceIdentifier;
        }

        public ServiceElement this[int index]
        {
            get
            {
                return BaseGet(index) as ServiceElement;
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ServiceElement this[string responseString]
        {
            get { return (ServiceElement)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }
    }
}
