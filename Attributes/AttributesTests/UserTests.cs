using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Attributes;
using System.Reflection;
using System.ComponentModel;

namespace AttributesTests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Ctor_CreateUserFromFirstAttribute_ReturnsUserWithCorrestProperties()
        {
            InstantiateUserAttribute[] allUserClassAttributes = (InstantiateUserAttribute[])Attribute.GetCustomAttributes(typeof(User), typeof(InstantiateUserAttribute));
            User newUser = TryToCreateUserFromAttribute(allUserClassAttributes[0]);
            Assert.AreEqual(1, newUser.Id);
            Assert.AreEqual("Alexander", newUser.FirstName);
            Assert.AreEqual("Alexandrov", newUser.LastName);
        }

        [TestMethod]
        public void Ctor_CreateUserFromSecondAttribute_ReturnsUserWithCorrestProperties()
        {
            InstantiateUserAttribute[] allUserClassAttributes = (InstantiateUserAttribute[])Attribute.GetCustomAttributes(typeof(User), typeof(InstantiateUserAttribute));
            User newUser = TryToCreateUserFromAttribute(allUserClassAttributes[1]);
            Assert.AreEqual(2, newUser.Id);
            Assert.AreEqual("Semen", newUser.FirstName);
            Assert.AreEqual("Semenov", newUser.LastName);
        }

        [TestMethod]
        public void Ctor_CreateUserFromThirdAttribute_ReturnsUserWithCorrestProperties()
        {
            InstantiateUserAttribute[] allUserClassAttributes = (InstantiateUserAttribute[])Attribute.GetCustomAttributes(typeof(User), typeof(InstantiateUserAttribute));
            User newUser = TryToCreateUserFromAttribute(allUserClassAttributes[2]);
            Assert.AreEqual(3, newUser.Id);
            Assert.AreEqual("Petr", newUser.FirstName);
            Assert.AreEqual("Petrov", newUser.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyValueException))]
        public void Ctor_CreateUserFromAttributeWithInvalidId_ThrowException()
        {
            InstantiateUserAttribute attribute = new InstantiateUserAttribute(-1, "John", "Smith");
            TryToCreateUserFromAttribute(attribute);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyValueException))]
        public void Ctor_CreateUserFromAttributeWithInvalidFirstName_ThrowException()
        {
            InstantiateUserAttribute attribute = new InstantiateUserAttribute(10, "Johnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn", "Smith");
            TryToCreateUserFromAttribute(attribute);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyValueException))]
        public void Ctor_CreateUserFromAttributeWithInvalidLastName_ThrowException()
        {
            InstantiateUserAttribute attribute = new InstantiateUserAttribute(10, "John", "Smithhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh");
            TryToCreateUserFromAttribute(attribute);
        }

        private User TryToCreateUserFromAttribute(InstantiateUserAttribute attribute)
        {
            Type userType = typeof(User);
            ConstructorInfo userCtor = userType.GetConstructor(new[] { typeof(int) });
            // User class attribute must contain 3 properties: Id, FirstName, Lastname.
            // But it is possible to create attribute without Id (attribute ctors allow it)
            // User ctor has required parameter (int id). This param is matching Id property, and this property
            // has default value. For getting default, we can:
            // 1. take param name from User ctor
            // 2. apply to MatchParameterAttribute to get needed property name
            // 3. get default value for found property name and give this value to User ctor.  
            if (attribute.Id == 0)
            {
                attribute.Id = GetDefaultValueOfRelatedProperty(userCtor);
            }
            if (!IdValuePassesValidation(attribute.Id))
            {
                throw new InvalidPropertyValueException("Id property value is invalid");
            }
            User newUser = (User)userCtor.Invoke(new object[] { attribute.Id });
            if (!FirstNameValuePassesValidation(attribute.FirstName))
            {
                throw new InvalidPropertyValueException("FirstName property value is invalid");
            }
            newUser.FirstName = attribute.FirstName;
            if (!LastNameValuePassesValidation(attribute.LastName))
            {
                throw new InvalidPropertyValueException("LastName property value is invalid");
            }
            newUser.LastName = attribute.LastName;
            return newUser;
        }

        private int GetDefaultValueOfRelatedProperty(ConstructorInfo userCtor)
        {
            var userCtorParameters = userCtor.GetParameters();
            MatchParameterWithPropertyAttribute[] attributesOfUserCtor = (MatchParameterWithPropertyAttribute[])userCtor.GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute), false);
            string neededPropertyName = null;
            int neededDefaultValue = 0;
            foreach (var attribute in attributesOfUserCtor)
            {
                if (attribute.ParameterName == userCtorParameters[0].Name)
                {
                    neededPropertyName = attribute.PropertyName;
                }
            }
            if (neededPropertyName != null)
            {
                AttributeCollection attributes = TypeDescriptor.GetProperties(typeof(User))[neededPropertyName].Attributes;
                DefaultValueAttribute myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
                neededDefaultValue = (int)myAttribute.Value;
            }
            return neededDefaultValue;
        }

        private bool IdValuePassesValidation(int idApplicant)
        {
            bool result = true;
            Type userType = typeof(User);
            var idField = userType.GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance);
            IntValidatorAttribute idValidationAttribute = (IntValidatorAttribute)idField.GetCustomAttribute(typeof(IntValidatorAttribute), false);
            if (idApplicant < idValidationAttribute.MinValue || idApplicant > idValidationAttribute.MaxValue)
            {
                result = false;
            }
            return result;
        }

        private bool FirstNameValuePassesValidation(string firstNameApplicant)
        {
            bool result = true;
            Type userType = typeof(User);
            var firstNameProperty = userType.GetProperty("FirstName");
            StringValidatorAttribute firstNameValidationAttribute = (StringValidatorAttribute)firstNameProperty.GetCustomAttribute(typeof(StringValidatorAttribute));
            if (firstNameApplicant.Length > firstNameValidationAttribute.MaxLength)
            {
                result = false;
            }
            return result;
        }

        private bool LastNameValuePassesValidation(string lastNameApplicant)
        {
            bool result = true;
            Type userType = typeof(User);
            var lastNameProperty = userType.GetProperty("LastName");
            StringValidatorAttribute lastNameValidationAttribute = (StringValidatorAttribute)lastNameProperty.GetCustomAttribute(typeof(StringValidatorAttribute));
            if (lastNameApplicant.Length > lastNameValidationAttribute.MaxLength)
            {
                result = false;
            }
            return result;
        }

    }
}
