namespace UserStorage.Services
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Predicates;
    using UserEntity;

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
