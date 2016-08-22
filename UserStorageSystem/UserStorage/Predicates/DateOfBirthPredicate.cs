namespace UserStorage.Predicates
{
    using System;
    using UserEntity;

    [Serializable]
    public class DateOfBirthPredicate : IPredicate
    {
        private readonly DateTime required;

        public DateOfBirthPredicate(DateTime requiredDate)
        {
            this.required = requiredDate;
        }

        public bool IsMatching(User user)
        {
            return user.DateOfBirth == this.required;
        }
    }
}
