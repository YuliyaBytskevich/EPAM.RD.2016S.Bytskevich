using System;
using System.Runtime.Serialization;
using UserStorage.UserEntity;

namespace UserStorage.Predicates
{
    [Serializable]
    [DataContract]
    public class LastNamePredicate: IPredicate
    {
        private readonly string required;

        public LastNamePredicate(string requiredName)
        {
            required = requiredName;
        }

        public bool IsMatching(User user)
        {
            return user.LastName == required;
        }
    }
}
