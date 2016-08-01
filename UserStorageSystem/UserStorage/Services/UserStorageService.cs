using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Configuration;
using System.ServiceProcess;
using NLog;
using UserStorage.Services;

namespace UserStorage
{
    public abstract class UserStorageService: IUserStorageService
    {
        protected ServiceState state = null;
        private static readonly Logger logger = LogManager.GetLogger("*");

        protected UserStorageService(string serviceIdentifier, string xmlPath, IUserStorage storage)
        {
            state = new ServiceState { identifier = serviceIdentifier, xmlPath = xmlPath, repository = (InMemoryUserStorage)storage };
        }

        public void Initialize()
        {
            // TODO: some actions to initialize service
            if (!string.IsNullOrEmpty(state.xmlPath))
            {
                RestoreServiceState();
            }
        }

        public void RestoreServiceState()
        {
            logger.Trace(state.identifier + " : restoring state from xml-file... ");
            try
            {
                var serializer = new XmlSerializer(typeof (ServiceState));
                using (Stream fStream = new FileStream(state.xmlPath, FileMode.Open))
                {
                    state = (ServiceState) serializer.Deserialize(fStream);
                }
                logger.Trace("state has been restored successfully!\n");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "\n");
            }
        }

        public void SaveServiceState()
        {
            logger.Trace(state.identifier + " : saving state to xml-file... ");
            try
            {
                if (state != null)
                {
                    var serializer = new XmlSerializer(typeof (ServiceState));
                    using (Stream fStream = new FileStream(state.xmlPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        serializer.Serialize(fStream, state);
                    }
                }
                logger.Trace("state has been saved successfully!\n");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "\n");
            }
        }

        public abstract void Add(User user);

        public abstract void Delete(User user);

        public int SearchForUser(params Func<User, bool>[] predicates)
        {
            logger.Trace(state.identifier + " : SEARCH [common service type] operation called... ");
            var result = state.repository.SearchForUser(predicates);
            if (result == -1)
                logger.Trace("Repository is empty.\n");
            else if (result == 0)
                logger.Trace("No matches found for given predicate(-s).\n");
            else
                logger.Trace("Found user ID: " + result + "\n");
            return result;
        }

        public IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates)
        {
            logger.Trace(state.identifier + " : SEARCH [common service type] operation called... ");
            var result = state.repository.SearchForUsers(predicates);
            if (result == null)
                logger.Trace("Repository is empty.\n");
            else if (!result.Any())
                logger.Trace("No matches found for given predicate(-s).\n");
            else
                logger.Trace("Found user ID: " + result + "\n");
            return result;
        }


    }
}
