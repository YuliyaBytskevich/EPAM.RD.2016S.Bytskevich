using System;
using System.ComponentModel;
using System.Reflection;

namespace Attributes
{
    // Should be applied to classes only.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InstantiateUserAttribute : Attribute
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public InstantiateUserAttribute() { }

        public InstantiateUserAttribute(string name, string lastname)
        {
            FirstName = name;
            LastName = lastname;
            //AttributeCollection attributes = TypeDescriptor.GetProperties(typeof(User))["Id"].Attributes;
            //DefaultValueAttribute myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
            //Id = (int)myAttribute.Value;
        }

        public InstantiateUserAttribute(int id, string name, string lastname)
        {
            Id = id;
            FirstName = name;
            LastName = lastname ;
        }
    }
}
