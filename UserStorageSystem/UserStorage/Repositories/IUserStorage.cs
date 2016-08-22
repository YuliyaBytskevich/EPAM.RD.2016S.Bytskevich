using System.Collections.Generic;
using System.Xml.Serialization;
using UserStorage.IdentifiersGeneration;
using UserStorage.Predicates;
using UserStorage.UserEntity;
using UserStorage.Validation;

namespace UserStorage.Repositories
{
    public interface IUserStorage : IXmlSerializable
    {
        int Add(User user, IUserValidation validationRules = null);

        int SearchForUser(params IPredicate[] predicates);

        List<int> SearchForUsers(params IPredicate[] predicates);

        void Delete(User user);

        void Delete(int id);

        void SetIdGenerator(IIdentifiersGenerator generator);
    }
}
