using System;
using UserStorage.UserEntity;

namespace UserStorage.Validation
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
