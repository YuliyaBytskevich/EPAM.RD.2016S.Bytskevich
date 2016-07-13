using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.UserStorageValidationExceptions
{
    public class InvalidLastNameException: InvalidUserInfoException
    {
        public InvalidLastNameException() { }

        public InvalidLastNameException(string message) : base(message) { }

        public InvalidLastNameException(string message, Exception inner) : base(message, inner) { }
    }
}
