// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyValidatorDatabase
    {
        private static Dictionary<Type, PropertyValidator> validatorsByAttributeType;

        static PropertyValidatorDatabase()
        {
            validatorsByAttributeType = new Dictionary<Type, PropertyValidator>
            {
                [typeof(MaxValueAttribute)] = new MaxValuePropertyValidator(),
                [typeof(MinValueAttribute)] = new MinValuePropertyValidator(),
                [typeof(RequiredAttribute)] = new RequiredPropertyValidator(),
                [typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator()
            };

        }

        public static PropertyValidator GetValidatorForAttribute(Type attributeType)
        {
            if (validatorsByAttributeType.TryGetValue(attributeType, out PropertyValidator validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}

