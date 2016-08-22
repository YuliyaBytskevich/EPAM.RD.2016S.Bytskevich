namespace UserStorage.UserEntity
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum Gender
    {
        [EnumMember]
        Male = 1,
        [EnumMember]
        Female
    }
}
