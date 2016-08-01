using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace UserStorage.Services
{
    public class SlaveService: UserStorageService
    {
        private static Logger logger = LogManager.GetLogger("*");

        public SlaveService (string serviceIdentifier, string xmlPath, IUserStorage storage): base(serviceIdentifier, xmlPath, storage) { }

        public override void Add(User user)
        {
            logger.Trace(state.identifier + " : ADD [slave service] operation called... ");
            logger.Error("Slave service is not allowed to call ADD operation");
            throw  new ForbiddenOperationException("Slave service is not allowed to call ADD operation");
        }

        public override void Delete(User user)
        {
            logger.Trace(state.identifier + " : DELETE [slave service] operation called... ");
            logger.Error("Slave service is not allowed to call DELETE operation");
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
                state.repository.Add(arguments.User);
            }
            else
            {
                state.repository.Delete(arguments.User);
            }
        }
    }
}
