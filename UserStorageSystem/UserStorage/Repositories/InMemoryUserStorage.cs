using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage
{
    //[Serializable]
    public class InMemoryUserStorage: IUserStorage
    {
        private List<User> users;
        private readonly IIdentifiersGenerator idGenerator;

        public InMemoryUserStorage()
        {
            int i = 10;
        }

        public InMemoryUserStorage(IIdentifiersGenerator generator)
        {
            users = new List<User>();
            idGenerator = generator;
        }

        protected InMemoryUserStorage(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            users = (List < User > ) info.GetValue("UsersList", typeof(List<User>));
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
                return -1;
            if (!predicates.Any())
                return users.First().Id;
            var commonPredicate = predicates[0];
            if (predicates.Count() > 1)
                commonPredicate = predicates.Aggregate(commonPredicate, (current, predicate) => current + predicate);
            var foundEntities = users.Where(x => commonPredicate(x));
            return foundEntities.Any() ? foundEntities.First().Id : 0;           
        }

        public IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates)
        {
            if (!users.Any())
                return null;
            if (!predicates.Any())
                return users.Select(user => user.Id);
            var commonPredicate = predicates[0];
            if (predicates.Count() > 1)
                commonPredicate = predicates.Aggregate(commonPredicate, (current, predicate) => current + predicate);
            var foundEntities = users.Where(x => commonPredicate(x));
            return foundEntities.Any() ? foundEntities.Select(user => user.Id) : new List<int>();
        }
        
        public void Delete(User user)
        {
            users.RemoveAll(x => x.Equals(user));
        }

        public void Delete(int id)
        {
            users.RemoveAll(x => x.Id == id);
        }

        public int GetUsersCount()
        {
            return users.Count();
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue("UsersList", users);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            users = (List<User>) serializer.Deserialize(reader);
            
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            serializer.Serialize(writer, users);
        }
    }
}
