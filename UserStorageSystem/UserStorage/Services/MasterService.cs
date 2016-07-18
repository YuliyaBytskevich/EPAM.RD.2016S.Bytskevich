using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Services;

namespace UserStorage
{
    public class MasterService : UserStorageService
    {
        public delegate void ChangesEventHandler(object sender, UserChangesEventArgs e);
        public event ChangesEventHandler ChangesHandler;

        public MasterService(IUserStorage storage) : base(storage) { }
        
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

    }
}
