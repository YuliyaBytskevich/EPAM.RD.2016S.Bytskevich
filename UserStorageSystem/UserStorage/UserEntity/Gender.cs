using System.Runtime.Serialization;

namespace UserStorage.UserEntity
{
    [DataContract]
    public enum Gender
    {
        [EnumMember]
        Male = 1,
        [EnumMember]
        Female
    }
}
