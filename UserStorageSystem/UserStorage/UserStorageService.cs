using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserStorage
{
    public class UserStorageService
    {
        private readonly IUserStorage storage;
        [Serializable]
        internal class ServiceState
        {
            internal int lastGeneratedId;
            internal List<User> users;
        }
        ServiceState state = null;

        public UserStorageService(IUserStorage storage)
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

        public virtual void Add(User user)
        {
            user.Id = storage.Add(user);
            state.users.Add(user);
            state.lastGeneratedId = user.Id;
        }

        public virtual void Delete(User user)
        {
            storage.Delete(user);
            state.users.RemoveAll(x => x.Equals(user));
        }

    }
}
