using System;

namespace UserStorage.Services
{
    public class ForbiddenOperationException: Exception
    {
        public ForbiddenOperationException(): base() { }

        public ForbiddenOperationException(string message) : base(message) { }

        public ForbiddenOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
