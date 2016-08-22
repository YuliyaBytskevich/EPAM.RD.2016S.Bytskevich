namespace UserStorage.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Xml.Serialization;
    using NLog;
    using Predicates;
    using Repositories;
    using UserEntity;

    public abstract class UserStorageService : MarshalByRefObject, IUserStorageService
    {
        private static readonly Logger Logger = LogManager.GetLogger("*");

        protected UserStorageService()
        {           
        }

        protected UserStorageService(string serviceIdentifier, string xmlPath, IUserStorage storage)
        {
            this.State = new ServiceState(serviceIdentifier, xmlPath, 0, storage);
        }

        public ManualResetEvent CollectionIsEnabled { protected get; set; } = new ManualResetEvent(true);

        public ServiceState State { get; set; }

        public void RestoreServiceState(IUserStorage targetStorage)
        {
            Logger.Trace(this.State.Identifier + " : restoring State from xml-file... ");
            try
            {
                var serializer = new XmlSerializer(typeof(ServiceState));
                using (Stream stream = new FileStream(this.State.XmlPath, FileMode.Open))
                {
                    this.State.SetTargerRepository(targetStorage);
                    this.State = (ServiceState)serializer.Deserialize(stream);
                }

                Logger.Trace("State has been restored successfully!\n");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace + "\n");
            }
        }

        public void SaveServiceState()
        {
            Logger.Trace(this.State.Identifier + " : saving State to xml-file... ");
            try
            {
                if (this.State != null)
                {
                    var serializer = new XmlSerializer(typeof(ServiceState));
                    using (Stream stream = new FileStream(this.State.XmlPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        serializer.Serialize(stream, this.State);
                    }
                }

                Logger.Trace("State has been saved successfully!\n");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace + "\n");
            }
        }

        public abstract void Add(User user);

        public abstract void Delete(User user);

        public virtual int SearchForUser(params IPredicate[] predicates)
        {
            Logger.Trace(this.State.Identifier + " : SEARCH operation called... ");
            var result = this.State.Repository.SearchForUser(predicates);
            if (result == -1)
            {
                Logger.Trace(this.State.Identifier + ": Repository is empty.\n");
            }               
            else if (result == 0)
            {
                Logger.Trace(this.State.Identifier + ": No matches found for given predicate(-s).\n");
            }
            else
            {
                Logger.Trace(this.State.Identifier + ": Found user ID: " + result + "\n");
            }

            return result;
        }

        public virtual List<int> SearchForUsers(params IPredicate[] predicates)
        {
            Logger.Trace(this.State.Identifier + " : SEARCH operation called... ");
            var result = this.State.Repository.SearchForUsers(predicates);
            if (result == null)
            {
                Logger.Trace(this.State.Identifier + ": Repository is empty.\n");
            }
            else if (!result.Any())
            {
                Logger.Trace(this.State.Identifier + ": No matches found for given predicate(-s).\n");
            }
            else
            {
                Logger.Trace(this.State.Identifier + ": Found user ID-s:");
                foreach (var element in result)
                {
                    Logger.Trace(element);
                }

                Logger.Trace("\n");
            }

            return result;
        }
    }
}
