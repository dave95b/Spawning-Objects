// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyMetaDatabase
    {
        private static Dictionary<Type, PropertyMeta> metasByAttributeType;

        static PropertyMetaDatabase()
        {
            metasByAttributeType = new Dictionary<Type, PropertyMeta>
            {
                [typeof(InfoBoxAttribute)] = new InfoBoxPropertyMeta(),
                [typeof(OnValueChangedAttribute)] = new OnValueChangedPropertyMeta()
            };

        }

        public static PropertyMeta GetMetaForAttribute(Type attributeType)
        {
            if (metasByAttributeType.TryGetValue(attributeType, out PropertyMeta meta))
            {
                return meta;
            }
            else
            {
                return null;
            }
        }
    }
}

