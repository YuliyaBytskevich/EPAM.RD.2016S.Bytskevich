using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.UserStorageValidationExceptions
{
    public class InvalidUserInfoException: Exception
    {
        public InvalidUserInfoException() { }

        public InvalidUserInfoException(string message) : base(message) { }

        public InvalidUserInfoException(string message, Exception inner) : base(message, inner) { }
    }
}
