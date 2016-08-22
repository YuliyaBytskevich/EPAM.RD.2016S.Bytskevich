using System;
using System.Linq;
using System.Runtime.Serialization;

namespace UserStorage.UserEntity
{
    [DataContract]
    [Serializable]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        [DataMember]
        public string PersonalId { get; set; }
        [DataMember]
        public Gender Gender { get; set; }
        [DataMember]
        public VisaRecord[] Visas { get; set; }

        public User() { }

        public User(string firstname, string lastname, DateTime dateOfBirth, string personalId, Gender gender, params VisaRecord[] visas)
        {
            FirstName = firstname;
            LastName = lastname;
            DateOfBirth = dateOfBirth;
            PersonalId = personalId;
            Gender = gender;
            Visas = visas;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            User user = obj as User;
            if ((System.Object)user == null)
            {
                return false;
            }
            return (PersonalId == user.PersonalId &&
                    DateOfBirth == user.DateOfBirth &&
                    Gender == user.Gender &&
                    LastName == user.LastName &&
                    FirstName == user.FirstName &&
                    AllVisasMatch(Visas, user.Visas)
                );
        }

        public override int GetHashCode()
        {
            return (PersonalId.GetHashCode() ^ DateOfBirth.GetHashCode() ^ LastName.GetHashCode());
        }

        private bool AllVisasMatch(VisaRecord[] firstUserVisas, VisaRecord[] secondUserVisas)
        {
            if (firstUserVisas == null && secondUserVisas == null)
            {
                return true;
            }
            if (firstUserVisas == null || secondUserVisas == null)
            {
                return false;
            }
            if (firstUserVisas.Count() != secondUserVisas.Count())
            {
                return false;
            }
            int numOfVisas = firstUserVisas.Count();
            for (int i = 0; i < numOfVisas; i++)
            {
                if (!firstUserVisas[i].Equals(secondUserVisas[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
