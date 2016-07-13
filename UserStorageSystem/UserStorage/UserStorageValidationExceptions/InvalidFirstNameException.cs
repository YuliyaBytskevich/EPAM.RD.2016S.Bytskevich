using System;

namespace UserStorage.UserStorageValidationExceptions
{
    public class InvalidFirstNameException: InvalidUserInfoException
    {
        public InvalidFirstNameException() {}

        public InvalidFirstNameException(string message) : base(message) { }

        public InvalidFirstNameException(string message, Exception inner) : base(message, inner) { }
    }
}
