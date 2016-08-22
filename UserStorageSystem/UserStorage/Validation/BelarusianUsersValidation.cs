namespace UserStorage.Validation
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UserEntity;

    public class BelarusianUsersValidation : MarshalByRefObject, IUserValidation
    {
        private readonly Regex firstNameRegex = new Regex("[A-Z][a-z]+");
        private readonly Regex lastNameRegex = new Regex("[A-Z][a-z]+(-[A-Z][a-z]+)?");
        private readonly Regex personalIdRegex = new Regex("[0-9]{7}[A-Z][0-9]{3}[A-Z]{2}[0-9]");
        
        public bool FirstNameIsValid(string firstNameApplicant)
        {
            return this.firstNameRegex.IsMatch(firstNameApplicant) ? true : false;
        }

        public bool LastNameIsValid(string lastNameApplicant)
        {
            return this.lastNameRegex.IsMatch(lastNameApplicant) ? true : false;
        }

        public bool DateOfBirthIsValid(DateTime dateOfBirthApplicant)
        {
            return (dateOfBirthApplicant < DateTime.Now && 
                    DateTime.Now.Year - dateOfBirthApplicant.Year < 150) ? true : false;
        }

        public bool PersonalIdIsValid(string personalIdApplicant)
        {
            return this.personalIdRegex.IsMatch(personalIdApplicant) ? true : false;
        }

        public bool VisaRecordsAreValid(VisaRecord[] visasApplicants)
        {
            if (visasApplicants == null)
            {
                return true;
            }

            return visasApplicants.All(this.VisaRecordIsValid);
        }

        private bool VisaRecordIsValid(VisaRecord visa)
        {
            return visa.DateOfEnding > visa.DateOfStarting;
        }
    }
}
