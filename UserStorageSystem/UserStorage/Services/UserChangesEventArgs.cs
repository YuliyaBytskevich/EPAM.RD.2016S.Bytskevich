using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Services
{
    public class UserChangesEventArgs
    {
        public User User { get; }
        public Operation Operation { get; }
        public UserChangesEventArgs(User user, Operation operation)
        {
            User = user;
            Operation = operation;
        }
    }

    public enum Operation1
    {
        Add,
        Remove
    }

}
