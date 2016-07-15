using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    public class InvalidPropertyValueException: Exception
    {
        public InvalidPropertyValueException() { }

        public InvalidPropertyValueException(string message):base(message) { }

        public InvalidPropertyValueException(string message, Exception inner):base(message, inner) { }
    }
}
