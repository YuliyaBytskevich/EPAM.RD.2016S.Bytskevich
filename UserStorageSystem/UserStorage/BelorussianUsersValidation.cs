using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserStorage
{
    public class BelorussianUsersValidation: IUserValidation
    {
        private Regex firstNameRegex = new Regex("[A-Z][a-z]+");
        private Regex lastNameRegex = new Regex("[A-Z][a-z]+(-[A-Z][a-z]+)?");
        private Regex personalIdRegex = new Regex("[0-9]{7}[A-Z][0-9]{3}[A-Z]{2}[0-9]");
        
        public bool FirstNameIsValid(string firstNameApplicant)
        {
            return firstNameRegex.IsMatch(firstNameApplicant) ? true : false;
        }

        public bool LastNameIsValid(string lastNameApplicant)
        {
            return lastNameRegex.IsMatch(lastNameApplicant) ? true : false;
        }

        public bool DateOfBirthIsValid(DateTime dateOfBirthApplicant)
        {
            return (dateOfBirthApplicant < DateTime.Now && 
                    DateTime.Now.Year - dateOfBirthApplicant.Year < 150) ? true : false;
        }

        public bool PersonalIdIsValid(string personalIdApplicant)
        {
            return personalIdRegex.IsMatch(personalIdApplicant) ? true : false;
        }

        public bool VisaRecordsAreValid(VisaRecord[] visasApplicants)
        {
            bool result = true;
            foreach(var visa in visasApplicants)
            {
                if (!VisaRecordIsValid(visa))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private bool VisaRecordIsValid(VisaRecord visa)
        {
            // TODO: 1. Also add checking by existing countries of the world
            return (visa.DateOfEnding > visa.DateOfStarting) ? true : false;
        }

    }
}
