using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IntValidatorAttribute : Attribute
    {
        public int MinValue { get; }
        public int MaxValue { get; }

        public IntValidatorAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
