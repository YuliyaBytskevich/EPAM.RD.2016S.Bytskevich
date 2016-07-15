using System;
using System.ComponentModel;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public int ExternalId { get; set; }

        public InstantiateAdvancedUserAttribute(string name, string lastname)
        {
            FirstName = name;
            LastName = lastname;
            AttributeCollection attributes = TypeDescriptor.GetProperties(typeof(User))["Id"].Attributes;
            DefaultValueAttribute myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
            Id = (int)myAttribute.Value;
            attributes = TypeDescriptor.GetProperties(typeof(AdvancedUser))["ExternalId"].Attributes;
            myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
            ExternalId = (int)myAttribute.Value;
        }

        public InstantiateAdvancedUserAttribute(int id, string name, string lastname, int externalId)
        {
            this.Id = id;
            this.FirstName = name;
            this.LastName = lastname;
            this.ExternalId = externalId;
        }
    }
}
