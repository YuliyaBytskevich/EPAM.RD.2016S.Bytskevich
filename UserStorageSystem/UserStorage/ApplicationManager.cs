using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using UserStorage.Configurations;
using UserStorage.IdentifiersGeneration;
using UserStorage.Repositories;
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
            string correctPath =
                @"C:\Users\julia\Desktop\epam\EPAM.RD.2016S.Bytskevich\UserStorageSystem\UserStorage\bin\Debug\UserStorage.dll.config";
            fileMap.ExeConfigFilename = correctPath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            ServicesConfigSection section = (ServicesConfigSection)config.GetSection("ServicesSection");          
            if (section != null)
            {
                int itemsCount = section.SectionItems.Count;
                
                for (int i = 0; i < itemsCount; i++)
                {
                    if (section.SectionItems[i].ServiceType == "master")
                    {
                        var newMaster = (MasterService)CreateService(section.SectionItems[i]);
                        Masters.Add(newMaster);
                    }
                    else if (section.SectionItems[i].ServiceType == "slave")
                    {
                        var newSlave = (SlaveService)CreateService(section.SectionItems[i]);
                        Slaves.Add(newSlave);
                        foreach (var masterServce in Masters)
                        {
                            masterServce.RegisterPortForSlaveService(section.SectionItems[i].Port);
                        }
                    }
                    else
                        throw new ApplicationException("Application manager doesn't support service type '" + section.SectionItems[i].ServiceType + "'.");

                }
                Console.WriteLine("ALL SERVICES ARE CONFIGURED");
            }
            else
                throw new ApplicationException("Configuration manager can't extract required information. Configuration file (App.config) is empty or currupted.");
        }

        private static UserStorageService CreateService(ServiceElement config)
        {
            AppDomain domain = AppDomain.CreateDomain("CustomDomain_" + config.ServiceIdentifier);
            domain.Load("UserStorage");
            PrimeNumbersGenerator generator = (PrimeNumbersGenerator)
                    domain.CreateInstanceAndUnwrap("UserStorage", "UserStorage.IdentifiersGeneration.PrimeNumbersGenerator",
                        false, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
                        null, new object[] { 1 }, null, null);
            InMemoryUserStorage storage = (InMemoryUserStorage)
                    domain.CreateInstanceAndUnwrap("UserStorage", "UserStorage.Repositories.InMemoryUserStorage",
                        false, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
                        null, new object[] { generator }, null, null);
            UserStorageService service;
            if (config.ServiceType == "master")
            {
                service = (MasterService)domain.CreateInstanceAndUnwrap("UserStorage", "UserStorage.Services.MasterService",
                false, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
                null, new object[] { config.ServiceIdentifier, config.XmlPath, storage }, null, null);
            }
            else
            {
                service = (SlaveService)domain.CreateInstanceAndUnwrap("UserStorage", "UserStorage.Services.SlaveService",
                false, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
                null, new object[] { config.ServiceIdentifier, config.XmlPath, config.Port, storage }, null, null);
            }
            return service;
        }
    }
}
