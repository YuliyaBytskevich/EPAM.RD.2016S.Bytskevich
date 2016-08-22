using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage;
using UserStorage.IdentifiersGeneration;
using UserStorage.Predicates;
using UserStorage.Repositories;
using UserStorage.UserEntity;

namespace UserStorageConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationManager.ConfigureAppServces();
            Console.WriteLine("Master services: " + ApplicationManager.Masters.Count);
            Console.WriteLine("Slave services: " + ApplicationManager.Slaves.Count);
            Console.WriteLine();

            Thread threadCallingMaster = new Thread(CallsToMasterService);
            threadCallingMaster.Start();
            Thread threadCallingFirstSlave = new Thread(CallsToFirstSlave);
            threadCallingFirstSlave.Start();
            Thread threadCallingSecondSlave = new Thread(CallsToSecondSlave);
            threadCallingSecondSlave.Start();

            Console.ReadLine();
        }

        public static void CallsToMasterService()
        {
            ApplicationManager.Masters[0].RestoreServiceState(new InMemoryUserStorage());
            ApplicationManager.Masters[0].State.Repository.SetIdGenerator(new PrimeNumbersGenerator(ApplicationManager.Masters[0].State.LastGeneratedId));
            User newUser = new User("Jerry", "Mouse", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male, new VisaRecord("Neverland", new DateTime(2014, 1, 1), new DateTime(2015, 1, 1)), new VisaRecord("Belarus", new DateTime(2014, 10, 10), new DateTime(2015, 10, 10)));
            ApplicationManager.Masters[0].Add(newUser);
            //ApplicationManager.Masters[0].Delete(newUser);
            ApplicationManager.Masters[0].SearchForUsers();
            ApplicationManager.Masters[0].SearchForUsers(new FirstNamePredicate("John"), new LastNamePredicate("Smith"));
        }

        public static void CallsToFirstSlave()
        {
            ApplicationManager.Slaves[0].SearchForUsers();
            ApplicationManager.Slaves[0].EnableNetworkConnection();
            try
            {
                User newUser = new User("Darth", "Vader", new DateTime(1990, 1, 1), "1111111A111PB1", Gender.Male,
                    new VisaRecord("Neverland", new DateTime(2014, 1, 1), new DateTime(2015, 1, 1)),
                    new VisaRecord("Belarus", new DateTime(2014, 10, 10), new DateTime(2015, 10, 10)));
                ApplicationManager.Slaves[0].Add(newUser);
            }
            catch
            {
                // do nothing
            }
            Thread.Sleep(1000);
            ApplicationManager.Slaves[0].SearchForUsers();
        }

        public static void CallsToSecondSlave()
        {
            ApplicationManager.Slaves[1].EnableNetworkConnection();
        }

    }
}
