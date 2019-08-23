// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class NativePropertyDrawerDatabase
    {
        private static Dictionary<Type, NativePropertyDrawer> drawersByAttributeType;

        static NativePropertyDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, NativePropertyDrawer>
            {
                [typeof(ShowNativePropertyAttribute)] = new ShowNativePropertyNativePropertyDrawer()
            };

        }

        public static NativePropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (drawersByAttributeType.TryGetValue(attributeType, out NativePropertyDrawer drawer))
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

