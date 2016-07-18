using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Services
{
    public class ForbiddenOperationException: Exception
    {
        public ForbiddenOperationException(): base() { }

        public ForbiddenOperationException(string message) : base(message) { }

        public ForbiddenOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
