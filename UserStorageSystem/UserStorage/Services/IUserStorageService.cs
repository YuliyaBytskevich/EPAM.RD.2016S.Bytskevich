using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration;
using System.Configuration.Install;

namespace UserStorage.Services
{
    [ServiceContract(Namespace = "http://UserStorage.Services")]
    public interface IUserStorageService
    {
        [OperationContract]
        void RestoreServiceState();

        [OperationContract]
        void SaveServiceState();

        [OperationContract]
        void Add(User user);

        [OperationContract]
        void Delete(User user);

        [OperationContract]
        int SearchForUser(params Func<User, bool>[] predicates);

        [OperationContract]
        IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates);
    }
}
