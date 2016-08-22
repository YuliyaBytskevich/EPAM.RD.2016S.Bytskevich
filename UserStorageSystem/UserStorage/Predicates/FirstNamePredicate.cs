namespace UserStorage.Predicates
{
    using System;
    using System.Runtime.Serialization;
    using UserEntity;

    [Serializable]
    [DataContract]
    public class FirstNamePredicate : IPredicate
    {
        private readonly string required;

        public FirstNamePredicate(string requiredName)
        {
            this.required = requiredName;
        }

        public bool IsMatching(User user)
        {
            return user.FirstName == this.required;
        }
    }
}
