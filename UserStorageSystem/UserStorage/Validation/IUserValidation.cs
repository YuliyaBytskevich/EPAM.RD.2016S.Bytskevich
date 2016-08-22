namespace UserStorage.Validation
{
    using System;
    using UserEntity;

    public interface IUserValidation
    {
        bool FirstNameIsValid(string firstNameApplicant);

        bool LastNameIsValid(string lastNameApplicant);

        bool DateOfBirthIsValid(DateTime dateOfBirthApplicant);

        bool PersonalIdIsValid(string personalIdApplicant);

        bool VisaRecordsAreValid(VisaRecord[] visasApplicants);
    }
}
