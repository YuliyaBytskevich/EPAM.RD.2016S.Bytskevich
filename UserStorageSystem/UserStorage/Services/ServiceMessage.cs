using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Services
{
    [Serializable]
    public class ServiceMessage
    {
        public User ChangingData { get; set; }
        public Operation Operation { get; set; }

        public ServiceMessage() { }

        public ServiceMessage(User changingUser, Operation operation)
        {
            ChangingData = changingUser;
            Operation = operation;
        }

        protected ServiceMessage(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            ChangingData = (User) info.GetValue("ChangingData", typeof (User));
            Operation = (Operation) info.GetValue("Operation", typeof (Operation));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue("ChangingData", ChangingData);
            info.AddValue("Operation", Operation);
        }
    }

    [Serializable]
    public enum Operation
    {
        Add,
        Remove
    }
}
