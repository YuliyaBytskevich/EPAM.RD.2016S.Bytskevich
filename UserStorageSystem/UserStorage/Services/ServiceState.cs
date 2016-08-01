using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserStorage.Services
{
    [Serializable]
    public class ServiceState
    {
        public string identifier;
        public string xmlPath;
        public int lastGeneratedId;
        public InMemoryUserStorage repository;
    }
}
