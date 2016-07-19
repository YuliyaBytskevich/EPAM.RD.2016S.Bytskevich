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
            internal int serviceId;
            internal int lastGeneratedId;
            internal List<User> users = new List<User>();
            internal ServiceState() { }
        }
        protected ServiceState state = null;

        protected UserStorageService(int serviceId, IUserStorage storage)
        {
            this.storage = storage;
            state = new ServiceState();
            state.serviceId = serviceId;
        }

        public void RestoreServiceState()
        {
            var serializer = new XmlSerializer(typeof(ServiceState));
            string path = GetStatePathFromSettings();
            if (path == null)
                path = string.Format("test_service_{0}.xml", state.serviceId);
            using (Stream fStream = new FileStream(path, FileMode.Open))
            {
                state = (ServiceState)serializer.Deserialize(fStream);
            }
        }

        public void SaveServiceState()
        {
            if (state != null)
            {
                var serializer = new XmlSerializer(typeof(ServiceState));
                string path = GetStatePathFromSettings();
                if (path == null)
                    path = string.Format("test_service_{0}.xml", state.serviceId);
                using (Stream fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    serializer.Serialize(fStream, state);
                }
            }
        }

        public abstract void Add(User user);

        public abstract void Delete(User user);

        public abstract string GetStatePathFromSettings();

        public int SearchForUser(params Func<User, bool>[] predicates)
        {
            return storage.SearchForUser(predicates);
        }
                
    }
}
