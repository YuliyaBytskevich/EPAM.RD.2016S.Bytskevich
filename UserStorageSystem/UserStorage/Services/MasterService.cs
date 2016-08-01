using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using UserStorage.Services;
using UserStorage.Configurations;

namespace UserStorage
{
    public class MasterService : UserStorageService
    {
        public delegate void ChangesEventHandler(object sender, UserChangesEventArgs e);
        public event ChangesEventHandler ChangesHandler;
        private static Logger logger = LogManager.GetLogger("*");

        public MasterService(string serviceIdentifier, string xmlPath, IUserStorage storage) : base(serviceIdentifier, xmlPath, storage) { }
        
        public override void Add(User user)
        {
            logger.Trace(state.identifier + " : ADD [master service] operation called... ");
            try
            {
                user.Id = state.repository.Add(user);
                state.lastGeneratedId = user.Id;
                ChangesHandler?.Invoke(this, new UserChangesEventArgs(user, Operation.Add));
                logger.Trace("New user is added successfully. New user ID = " + state.lastGeneratedId + "\n");
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace + "\n");
            }
        }

        public override void Delete(User user)
        {
            logger.Trace(state.identifier + " : DELETE [master service] operation called... ");
            try
            {
                int id = user.Id;
                state.repository.Delete(user);
                ChangesHandler?.Invoke(this, new UserChangesEventArgs(user, Operation.Remove));
                logger.Trace("User with id " + id + "is deleted successfully.\n");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "\n");
            }
        }

    }
}
