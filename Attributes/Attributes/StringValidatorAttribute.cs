using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    public class StringValidatorAttribute : Attribute
    {
        public int MaxLength { get; }

        public StringValidatorAttribute(int maxLebngth)
        {
            MaxLength = maxLebngth;
        }
    }
}
