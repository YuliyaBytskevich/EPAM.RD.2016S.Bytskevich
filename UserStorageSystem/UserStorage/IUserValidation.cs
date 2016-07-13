using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public interface IUserValidation
    {

        bool FirstNameIsValid(string firstNameApplicant);

        bool LastNameIsValid(string lastNameApplicant);

        bool DateOfBirthIsValid(DateTime dateOfBirthApplicant);

        bool PersonalIdIsValid(string personalIdApplicant);

        bool VisaRecordsAreValid(VisaRecord[] visasApplicants);
    }
}
