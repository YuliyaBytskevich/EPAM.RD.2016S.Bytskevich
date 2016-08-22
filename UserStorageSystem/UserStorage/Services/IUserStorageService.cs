using System.Collections.Generic;
using System.ServiceModel;
using UserStorage.Predicates;
using UserStorage.UserEntity;

namespace UserStorage.Services
{
    [ServiceContract]
    public interface IUserStorageService
    {
        [OperationContract]
        void Add(User user);

        [OperationContract]
        void Delete(User user);

        [OperationContract]
        int SearchForUser(params IPredicate[] predicates);

        [OperationContract]
        List<int> SearchForUsers(params IPredicate[] predicates);
    }
}
