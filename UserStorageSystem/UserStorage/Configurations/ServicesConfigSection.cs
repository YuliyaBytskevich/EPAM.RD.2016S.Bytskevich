namespace UserStorage.Configurations
{
    using System.Configuration;

    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServicesCollection), AddItemName = "add")]
        public ServicesCollection SectionItems => (ServicesCollection)this["Services"];

        public ServicesCollection ServicesCollection
        {
            get
            {
                object o = this["Services"];
                return o as ServicesCollection;
            }
        }

        public static ServicesConfigSection GetConfig()
        {
            return (ServicesConfigSection)System.Configuration.ConfigurationManager.GetSection("ServicesSection") ?? new ServicesConfigSection();
        }
    }
}
