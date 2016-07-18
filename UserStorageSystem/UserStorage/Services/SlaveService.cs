using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Services
{
    public class SlaveService: UserStorageService
    {
        public SlaveService (IUserStorage storage): base(storage) { }

        public override void Add(User user)
        {
            throw  new ForbiddenOperationException("Slave service is not allowed to call ADD operation");
        }

        public override void Delete(User user)
        {
            throw new ForbiddenOperationException("Slave service is not allowed to call DELETE operation");
        }

        public void SubscribeForMasterActivity(MasterService master)
        {
            master.ChangesHandler += ProcessChanges;
        }

        public void UnsubscribeForMasterActivity(MasterService master)
        {
            master.ChangesHandler -= ProcessChanges;
        }

        private void ProcessChanges(object sender, UserChangesEventArgs arguments)
        {
            if (arguments.Operation == Operation.Add)
            {
                state.users.Add(arguments.User);
            }
            else
            {
                state.users.Remove(arguments.User);
            }
        }
    }
}
