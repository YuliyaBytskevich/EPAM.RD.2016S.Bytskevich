using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public class InMemoryUserStorage: IUserStorage
    {
        private List<User> users;
        private readonly IIdentifiersGenerator idGenerator;

        public InMemoryUserStorage(IIdentifiersGenerator generator)
        {
            users = new List<User>();
            idGenerator = generator;
        }

        public int Add(User user, IUserValidation validationRules = null) 
        {
            // TODO: check for IDs repeat. It can happen after generator reset.
            if (validationRules != null)
            {
                string exceptionMessage = null;
                bool userInfoIsValid = true;
                if(!validationRules.FirstNameIsValid(user.FirstName))
                {
                    userInfoIsValid = false;
                    exceptionMessage += "User's firstname is invalid.\n";
                }
                if (!validationRules.LastNameIsValid(user.LastName))
                {
                    userInfoIsValid = false;
                    exceptionMessage += "User's lastname is invalid.\n";
                }
                if (!validationRules.DateOfBirthIsValid(user.DateOfBirth))
                {
                    userInfoIsValid = false;
                    exceptionMessage += "User's date of birth is invalid.\n";
                }
                if (!validationRules.PersonalIdIsValid(user.PersonalId))
                {
                    userInfoIsValid = false;
                    exceptionMessage += "User's personal ID is invalid.\n";
                }
                if (!validationRules.VisaRecordsAreValid(user.Visas))
                {
                    userInfoIsValid = false;
                    exceptionMessage += "Information about user's vivas is invalid.\n";
                }
                if (!userInfoIsValid)
                    throw new UserStorageValidationExceptions.InvalidUserInfoException(exceptionMessage);
                user.Id = idGenerator.GenerateNewNumber();
                users.Add(user);
                return user.Id;
            }           
            user.Id = idGenerator.GenerateNewNumber();
            users.Add(user);
            return user.Id;
        }

        public int SearchForUser(params Func<User, bool>[] predicates)
        {
            if (!users.Any())
                return 0;
            if (predicates.Count() == 0)
                return users.First().Id;
            Func<User, bool> commonPredicate = predicates[0];
            if (predicates.Count() > 1)
                foreach (var predicate in predicates)
                    commonPredicate += predicate;
            var foundEntities = users.Where(x => commonPredicate(x));
            return foundEntities.Any() ? foundEntities.First().Id : 0;           
       }

        public void Delete(User user)
        {
            users.RemoveAll(x => x.Equals(user));
        }

        public void Delete(int id)
        {
            users.RemoveAll(x => x.Id == id);
        }

    }
}
