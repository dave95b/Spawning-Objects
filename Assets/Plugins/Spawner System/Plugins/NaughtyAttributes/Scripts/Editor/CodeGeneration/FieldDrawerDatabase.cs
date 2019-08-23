// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class FieldDrawerDatabase
    {
        private static Dictionary<Type, FieldDrawer> drawersByAttributeType;

        static FieldDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, FieldDrawer>
            {
                [typeof(ShowNonSerializedFieldAttribute)] = new ShowNonSerializedFieldFieldDrawer()
            };

        }

        public static FieldDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (drawersByAttributeType.TryGetValue(attributeType, out FieldDrawer drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}

