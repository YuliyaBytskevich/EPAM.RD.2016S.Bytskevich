using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.UserStorageValidationExceptions
{
    public class InvalidVisaRecordsException: InvalidUserInfoException
    {
        public InvalidVisaRecordsException() { }

        public InvalidVisaRecordsException(string message) : base(message) { }

        public InvalidVisaRecordsException(string message, Exception inner) : base(message, inner) { }
    }
}
