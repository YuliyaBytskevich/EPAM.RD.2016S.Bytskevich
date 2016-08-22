using System;
using UserStorage.UserEntity;

namespace UserStorage.Predicates
{
    [Serializable]
    public class DateOfBirthPredicate: IPredicate
    {
        private readonly DateTime required;

        public DateOfBirthPredicate(DateTime requiredDate)
        {
            required = requiredDate;
        }

        public bool IsMatching(User user)
        {
            return user.DateOfBirth == required;
        }
    }
}
