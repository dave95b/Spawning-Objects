// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyDrawConditionDatabase
    {
        private static Dictionary<Type, PropertyDrawCondition> drawConditionsByAttributeType;

        static PropertyDrawConditionDatabase()
        {
            drawConditionsByAttributeType = new Dictionary<Type, PropertyDrawCondition>
            {
                [typeof(HideIfAttribute)] = new HideIfPropertyDrawCondition(),
                [typeof(ShowIfAttribute)] = new ShowIfPropertyDrawCondition()
            };

        }

        public static PropertyDrawCondition GetDrawConditionForAttribute(Type attributeType)
        {
            if (drawConditionsByAttributeType.TryGetValue(attributeType, out PropertyDrawCondition drawCondition))
            {
                return drawCondition;
            }
            else
            {
                return null;
            }
        }
    }
}

