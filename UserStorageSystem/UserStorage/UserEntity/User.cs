namespace UserStorage.UserEntity
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    public class User
    {
        public User()
        {           
        }

        public User(string firstname, string lastname, DateTime dateOfBirth, string personalId, Gender gender, params VisaRecord[] visas)
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.DateOfBirth = dateOfBirth;
            this.PersonalId = personalId;
            Gender = gender;
            this.Visas = visas;
        }

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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            User user = obj as User;
            if ((object)user == null)
            {
                return false;
            }

            return this.PersonalId == user.PersonalId &&
                   this.DateOfBirth == user.DateOfBirth &&
                   Gender == user.Gender &&
                   this.LastName == user.LastName &&
                   this.FirstName == user.FirstName &&
                   this.AllVisasMatch(this.Visas, user.Visas);
        }

        public override int GetHashCode()
        {
            return this.PersonalId.GetHashCode() ^ this.DateOfBirth.GetHashCode() ^ this.LastName.GetHashCode();
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
