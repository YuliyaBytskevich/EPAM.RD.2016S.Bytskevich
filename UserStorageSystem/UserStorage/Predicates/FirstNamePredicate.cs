using System;
using System.Runtime.Serialization;
using UserStorage.UserEntity;

namespace UserStorage.Predicates
{
    [Serializable]
    [DataContract]
    public class FirstNamePredicate: IPredicate
    {
        private readonly string required;

        public FirstNamePredicate(string requiredName)
        {
            required = requiredName;
        }

        public bool IsMatching(User user)
        {
            return user.FirstName == required;
        }
    }
}
