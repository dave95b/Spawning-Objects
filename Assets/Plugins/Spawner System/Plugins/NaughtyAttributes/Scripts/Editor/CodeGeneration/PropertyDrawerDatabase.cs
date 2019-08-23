// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyDrawerDatabase
    {
        private static Dictionary<Type, PropertyDrawer> drawersByAttributeType;

        static PropertyDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, PropertyDrawer>
            {
                [typeof(DisableIfAttribute)] = new DisableIfPropertyDrawer(),
                [typeof(DropdownAttribute)] = new DropdownPropertyDrawer(),
                [typeof(EnableIfAttribute)] = new EnableIfPropertyDrawer(),
                [typeof(MinMaxSliderAttribute)] = new MinMaxSliderPropertyDrawer(),
                [typeof(ProgressBarAttribute)] = new ProgressBarPropertyDrawer(),
                [typeof(ReadOnlyAttribute)] = new ReadOnlyPropertyDrawer(),
                [typeof(ReorderableListAttribute)] = new ReorderableListPropertyDrawer(),
                [typeof(ResizableTextAreaAttribute)] = new ResizableTextAreaPropertyDrawer(),
                [typeof(ShowAssetPreviewAttribute)] = new ShowAssetPreviewPropertyDrawer(),
                [typeof(SliderAttribute)] = new SliderPropertyDrawer()
            };

        }

        public static PropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            if (drawersByAttributeType.TryGetValue(attributeType, out PropertyDrawer drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }

        public static void ClearCache()
        {
            foreach (var kvp in drawersByAttributeType)
            {
                kvp.Value.ClearCache();
            }
        }
    }
}

