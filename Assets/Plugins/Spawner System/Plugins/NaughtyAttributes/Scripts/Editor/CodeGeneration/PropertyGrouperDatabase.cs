// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyGrouperDatabase
    {
        private static Dictionary<Type, PropertyGrouper> groupersByAttributeType;

        static PropertyGrouperDatabase()
        {
            groupersByAttributeType = new Dictionary<Type, PropertyGrouper>
            {
                [typeof(BoxGroupAttribute)] = new BoxGroupPropertyGrouper()
            };

        }

        public static PropertyGrouper GetGrouperForAttribute(Type attributeType)
        {
            if (groupersByAttributeType.TryGetValue(attributeType, out PropertyGrouper grouper))
            {
                return grouper;
            }
            else
            {
                return null;
            }
        }
    }
}

