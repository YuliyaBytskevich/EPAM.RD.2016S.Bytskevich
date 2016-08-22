namespace UserStorage.Predicates
{
    using System;
    using System.Runtime.Serialization;
    using UserEntity;

    [Serializable]
    [DataContract]
    public class LastNamePredicate : IPredicate
    {
        private readonly string required;

        public LastNamePredicate(string requiredName)
        {
            this.required = requiredName;
        }

        public bool IsMatching(User user)
        {
            return user.LastName == this.required;
        }
    }
}
