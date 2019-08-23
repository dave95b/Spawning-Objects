// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class MethodDrawerDatabase
    {
        private static Dictionary<Type, MethodDrawer> drawersByAttributeType;

        static MethodDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, MethodDrawer>
            {
                [typeof(ButtonAttribute)] = new ButtonMethodDrawer()
            };

        }

        public static MethodDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (drawersByAttributeType.TryGetValue(attributeType, out MethodDrawer drawer))
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

