using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.UserStorageValidationExceptions
{
    public class InvalidPersonalIdException: InvalidUserInfoException
    {
        public InvalidPersonalIdException() { }

        public InvalidPersonalIdException(string message) : base(message) { }

        public InvalidPersonalIdException(string message, Exception inner) : base(message, inner) { }
    }
}
