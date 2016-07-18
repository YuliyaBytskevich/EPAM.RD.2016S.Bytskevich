using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Configuration;

namespace UserStorage
{
    public abstract class UserStorageService
    {
        protected readonly IUserStorage storage;
        [Serializable]
        protected internal class ServiceState
        {
            internal int lastGeneratedId;
            internal List<User> users;
        }
        protected ServiceState state = null;

        protected UserStorageService(IUserStorage storage)
        {
            this.storage = storage;
        }

        public void RestoreServiceState()
        {
            var serializer = new XmlSerializer(typeof(ServiceState));
            using (Stream fStream = new FileStream("test.xml", FileMode.Open))
            {
                state = (ServiceState)serializer.Deserialize(fStream);
            }
        }

        public void SaveServiceState()
        {
            if (state != null)
            {
                var serializer = new XmlSerializer(typeof(ServiceState));
                using (Stream fStream = new FileStream("test.xml", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    serializer.Serialize(fStream, state);
                }
            }
        }

        public abstract void Add(User user);

        public abstract void Delete(User user);

        public int SearchForUser(params Func<User, bool>[] predicates)
        {
            return storage.SearchForUser(predicates);
        }

    }
}
