using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.UserStorageValidationExceptions
{
    public class InvalidDateOfBirthException: InvalidUserInfoException
    {
        public InvalidDateOfBirthException() { }

        public InvalidDateOfBirthException(string message) : base(message) { }

        public InvalidDateOfBirthException(string message, Exception inner) : base(message, inner) { }
    }
}
