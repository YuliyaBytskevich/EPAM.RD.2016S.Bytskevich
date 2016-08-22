namespace UserStorageWcfHosting
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using UserStorage;
    using UserStorage.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            ApplicationManager.ConfigureAppServces();
            List<ServiceHost> hosts = new List<ServiceHost>();
            int basePort = 9000;
            foreach (var master in ApplicationManager.Masters)
            {
                Uri address = new Uri("http://localhost:" + basePort + "/" + master.State.Identifier);
                basePort++;
                ServiceHost host = new ServiceHost(typeof(MasterService), address);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                hosts.Add(host);
                host.Open();
                Console.WriteLine("Master service host at [localhost:" + (basePort - 1) + "/" + master.State.Identifier + "] is opened.");
            }

            foreach (var slave in ApplicationManager.Slaves)
            {
                Uri address = new Uri("http://localhost:" + basePort + "/" + slave.State.Identifier);
                basePort++;
                ServiceHost host = new ServiceHost(typeof(SlaveService), address);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                hosts.Add(host);
                host.Open();
                Console.WriteLine("Slave service host at [localhost:" + (basePort - 1) + "/" + slave.State.Identifier + "] is opened.");
            }

            Console.WriteLine("Press any key to stop services...");
            Console.ReadLine();
            foreach (var host in hosts)
            {
                host.Close();
            }
        }
    }
}
