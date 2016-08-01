using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage.Configurations;
using UserStorage.Services;

namespace UserStorage
{
    public static class ApplicationManager
    {
        public static List<MasterService> Masters  { get; } = new List<MasterService>();
        public  static List<SlaveService> Slaves { get; } = new List<SlaveService>();

        public static void ConfigureAppServces()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename =
                typeof (ApplicationManager).Assembly.Location.Replace("UserStorageConsoleApp", "UserStorage") +
                ".config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            ServicesConfigSection section = (ServicesConfigSection)config.GetSection("ServicesSection");          
            if (section != null)
            {
                int itemsCount = section.SectionItems.Count;
                Thread[] serviceThreads = new Thread[itemsCount];
                for (int i = 0; i < itemsCount; i++)
                {
                    if (section.SectionItems[i].ServiceType == "master")
                    {
                        // TODO: run master service
                        var newMaster = new MasterService(section.SectionItems[i].ServiceIdentifier, section.SectionItems[i].XmlPath, new InMemoryUserStorage(new PrimeNumbersGenerator()));
                        Masters.Add(newMaster);
                        serviceThreads[i] = new Thread(CallsToMasterService);
                        //serviceThreads[i].Start();
                    }
                    else if (section.SectionItems[i].ServiceType == "slave")
                    {
                        // TODO: run slave service
                        var newSlave = new SlaveService(section.SectionItems[i].ServiceIdentifier, section.SectionItems[i].XmlPath, new InMemoryUserStorage(new PrimeNumbersGenerator()));
                        Slaves.Add(newSlave);
                        //serviceThreads[i] = new Thread(newSlave.Initialize);
                        //serviceThreads[i].Start();
                    }
                    else
                        throw new ApplicationException("Application manager doesn't support service type '" + section.SectionItems[i].ServiceType + "'.");

                }
                Console.WriteLine("ALL SERVICES ARE CONFIGURED");
                foreach (var thread in serviceThreads)
                {
                    thread?.Start();
                }
            }
            else
                throw new ApplicationException("Configuration manager can't extract required information. Configuration file (App.config) is empty or currupted.");
        }

        public static void CallsToMasterService()
        {
            Masters[0].RestoreServiceState();
            User newUser = new User("John", "Smith", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, new VisaRecord("Neverland", new DateTime(2014, 1, 1), new DateTime(2015, 1, 1)));
            Masters[0].Add(newUser);
            Masters[0].SaveServiceState();
        }

    }
}
