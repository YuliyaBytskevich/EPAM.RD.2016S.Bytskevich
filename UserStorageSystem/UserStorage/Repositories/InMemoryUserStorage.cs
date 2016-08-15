using System;
using System.Collections;
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
        private IIdentifiersGenerator idGenerator;

        public InMemoryUserStorage()
        {
   
        }

        public InMemoryUserStorage(IIdentifiersGenerator generator)
        {
            users = new List<User>();
            idGenerator = generator;
        }

        public void SetIdGenerator(IIdentifiersGenerator generator)
        {
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
            IEnumerable<User> foundEntities;
            if (predicates.Length == 1)
            {
                foundEntities = users.Where(x => predicates[0](x));
            }
            else
            {
                Func<User, bool> combinedPredicate = x => true;
                combinedPredicate = predicates.Aggregate(combinedPredicate, (current, predicate) => (x => (current(x) && predicate(x))));
                foundEntities = users.Where(combinedPredicate);
            }
            return foundEntities.Any() ? foundEntities.First().Id : 0;           
        }

        public IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates)
        {
            if (!users.Any())
                return null;
            if (!predicates.Any())
                return users.Select(user => user.Id);
            IEnumerable<User> foundEntities;
            if (predicates.Length == 1)
            {
                foundEntities = users.Where(x => predicates[0](x));
            }
            else
            {
                Func<User, bool> combinedPredicate = x => true;
                combinedPredicate = predicates.Aggregate(combinedPredicate, (current, predicate) => (x => (current(x) && predicate(x))));
                foundEntities = users.Where(combinedPredicate);
            }
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            users = new List<User>();
            while (reader.Read())
            {
                if (reader.Name.Equals("User") && (reader.IsStartElement()))
                {
                    User newUser = new User();
                    reader.Read();
                    newUser.Id = reader.ReadElementContentAsInt();
                    newUser.FirstName = reader.ReadElementContentAsString();
                    newUser.LastName = reader.ReadElementContentAsString();
                    newUser.DateOfBirth = DateTime.Parse(reader.ReadElementContentAsString());
                    newUser.PersonalId = reader.ReadElementContentAsString();
                    string genderTemp = reader.ReadElementContentAsString();
                    newUser.Gender = genderTemp == "Male" ? Gender.Male : Gender.Female;
                    if (!reader.IsEmptyElement)
                    {
                        reader.Read();
                        List<VisaRecord> tempVisasList = new List<VisaRecord>();
                        while (reader.Name != "Visa" && reader.IsStartElement())
                        {
                            var newVisa = new VisaRecord();
                            newVisa.ReadXml(reader);
                            tempVisasList.Add(newVisa);
                        }
                        newUser.Visas = tempVisasList.ToArray();
                    }
                    users.Add(newUser);
                }
            }

        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Repository");
            writer.WriteStartElement("ArrayOfUser");
            foreach (var user in users)
            {
                writer.WriteStartElement("User");
                writer.WriteElementString("Id", user.Id.ToString());
                writer.WriteElementString("FirstName", user.FirstName);
                writer.WriteElementString("LastName", user.LastName);
                writer.WriteElementString("DateOfBirth", user.DateOfBirth.ToString());
                writer.WriteElementString("PersonalId", user.PersonalId);
                writer.WriteElementString("Gender", user.Gender.ToString());
                writer.WriteStartElement("Visas");
                if (user.Visas != null)
                {
                    foreach (var visa in user.Visas)
                    {
                        visa.WriteXml(writer);
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
