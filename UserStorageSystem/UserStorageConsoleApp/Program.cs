using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace UserStorageConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationManager.ConfigureAppServces();
            Console.WriteLine("Master services: " + ApplicationManager.Masters.Count);
            Console.WriteLine("Slave services: " + ApplicationManager.Slaves.Count);
            Console.ReadLine();
        }
    }
}
