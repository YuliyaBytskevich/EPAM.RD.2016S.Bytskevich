using Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributesTests
{
    [TestClass]
    public class AdvansedUserTests
    {
        [TestMethod]
        public void Ctor_CreateAdvansedUserFromAttributeWithNoIdAndExternalId_ReturnCreatedUserWithDefaultIdAndExeternalId()
        {
            InstantiateAdvancedUserAttribute attribute = new InstantiateAdvancedUserAttribute("John", "Smith");
            AdvancedUser newAdvansedUser = TryToCreateAdvansedUserFromAttribute(attribute);
            Assert.AreEqual(1, newAdvansedUser.Id);
            Assert.AreEqual("John", newAdvansedUser.FirstName);
            Assert.AreEqual("Smith", newAdvansedUser.LastName);
            Assert.AreEqual(3443454, newAdvansedUser.ExternalId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyValueException))]
        public void Ctor_CreateAdvansedUserFromAttributeWithInvalidId_ReturnCreatedUserWithDefaultIdAndExeternalId()
        {
            InstantiateAdvancedUserAttribute attribute = new InstantiateAdvancedUserAttribute(-1, "John", "Smith", 100);
            TryToCreateAdvansedUserFromAttribute(attribute);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyValueException))]
        public void Ctor_CreateAdvansedUserFromAttributeWithInvalidFirstName_ReturnCreatedUserWithDefaultIdAndExeternalId()
        {
            InstantiateAdvancedUserAttribute attribute = new InstantiateAdvancedUserAttribute(1, "Johnnnnnnnnnnnnnnnnnnnnnnnnnnnnn", "Smith", 100);
            TryToCreateAdvansedUserFromAttribute(attribute);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyValueException))]
        public void Ctor_CreateAdvansedUserFromAttributeWithInvalidLastName_ReturnCreatedUserWithDefaultIdAndExeternalId()
        {
            InstantiateAdvancedUserAttribute attribute = new InstantiateAdvancedUserAttribute(1, "John", "Smithhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh", 100);
            TryToCreateAdvansedUserFromAttribute(attribute);
        }

        private AdvancedUser TryToCreateAdvansedUserFromAttribute(InstantiateAdvancedUserAttribute attribute)
        {
            Type advansedUserType = typeof(AdvancedUser);
            ConstructorInfo advansedUserCtor = advansedUserType.GetConstructor(new[] { typeof(int), typeof(int) });
            var advansedUserCtorParameters = advansedUserCtor.GetParameters();
            if (attribute.Id == 0)
            {
                attribute.Id = GetDefaultValueOfParameterRelatedProperty(advansedUserCtor, advansedUserCtorParameters[0]);
            }
            if (attribute.ExternalId == 0)
            {
                attribute.ExternalId = GetDefaultValueOfParameterRelatedProperty(advansedUserCtor, advansedUserCtorParameters[1]);
            }
            if (!IdValuePassesValidation(attribute.Id))
            {
                throw new InvalidPropertyValueException("Id property value is invalid");
            }
            AdvancedUser newUser = (AdvancedUser)advansedUserCtor.Invoke(new object[] { attribute.Id, attribute.ExternalId });
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

        private int GetDefaultValueOfParameterRelatedProperty(ConstructorInfo advansedUserCtor, ParameterInfo parameter)
        {
            MatchParameterWithPropertyAttribute[] attributesOfAdvansedUserCtor = (MatchParameterWithPropertyAttribute[])advansedUserCtor.GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute), false);
            string neededPropertyName = null; 
            int neededDefaultValue = 0;
            foreach (var attribute in attributesOfAdvansedUserCtor)
            {
                if (attribute.ParameterName == parameter.Name)
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
