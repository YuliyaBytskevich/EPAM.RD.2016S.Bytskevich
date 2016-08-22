namespace UserStorage.Repositories
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using IdentifiersGeneration;
    using Predicates;
    using UserEntity;
    using Validation;

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
