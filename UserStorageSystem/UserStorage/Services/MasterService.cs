using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Services;
using UserStorage.Configurations;

namespace UserStorage
{
    public class MasterService : UserStorageService
    {
        public delegate void ChangesEventHandler(object sender, UserChangesEventArgs e);
        public event ChangesEventHandler ChangesHandler;

        public MasterService(int serviceId, IUserStorage storage) : base(serviceId, storage) { }
        
        public override void Add(User user)
        {
            user.Id = storage.Add(user);
            state.users.Add(user);
            state.lastGeneratedId = user.Id;
            ChangesHandler?.Invoke(this, new UserChangesEventArgs(user, Operation.Add));
        }

        public override void Delete(User user)
        {
            storage.Delete(user);
            state.users.RemoveAll(x => x.Equals(user));
            ChangesHandler?.Invoke(this, new UserChangesEventArgs(user, Operation.Remove));
        }

        public override string GetStatePathFromSettings()
        {
           // ServiceStatePathConfigSection section = (ServiceStatePathConfigSection)ConfigurationManager.GetSection("ServiceStatePathSettings");
            string key = String.Format("master_{0}_path", state.serviceId);
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] ?? null;
        }

    }
}
