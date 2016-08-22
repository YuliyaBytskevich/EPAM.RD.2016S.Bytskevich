namespace UserStorage.Configurations
{
    using System.Configuration;

    [ConfigurationCollection(typeof(ServiceElement))]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public new ServiceElement this[string responseString]
        {
            get
            {
                return (ServiceElement)BaseGet(responseString);
            }

            set
            {
                if (this.BaseGet(responseString) != null)
                {
                    this.BaseRemoveAt(this.BaseIndexOf(this.BaseGet(responseString)));
                }

                this.BaseAdd(value);
            }
        }

        public ServiceElement this[int index]
        {
            get
            {
                return BaseGet(index) as ServiceElement;
            }

            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).ServiceIdentifier;
        }
    }
}
