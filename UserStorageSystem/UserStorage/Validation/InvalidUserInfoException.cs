namespace UserStorage.Validation
{
    using System;

    public class InvalidUserInfoException : Exception
    {
        public InvalidUserInfoException()
        {
        }

        public InvalidUserInfoException(string message) : base(message)
        {
        }

        public InvalidUserInfoException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
