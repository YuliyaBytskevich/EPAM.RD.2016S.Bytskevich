namespace UserStorage.Services
{
    using System;
    using System.Runtime.Serialization;
    using UserEntity;

    [Serializable]
    public enum Operation
    {
        Add,
        Remove
    }

    [Serializable]
    public class ServiceMessage
    {
        public ServiceMessage()
        {          
        }

        public ServiceMessage(User changingUser, Operation operation)
        {
            this.ChangingData = changingUser;
            Operation = operation;
        }

        protected ServiceMessage(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            this.ChangingData = (User)info.GetValue("ChangingData", typeof(User));
            Operation = (Operation)info.GetValue("Operation", typeof(Operation));
        }

        public User ChangingData { get; set; }

        public Operation Operation { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("ChangingData", this.ChangingData);
            info.AddValue("Operation", Operation);
        }
    }
}
