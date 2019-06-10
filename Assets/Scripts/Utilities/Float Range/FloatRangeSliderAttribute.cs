using UnityEngine;
using System.Collections.Generic;

namespace Core
{
    public class FloatRangeSliderAttribute : PropertyAttribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public FloatRangeSliderAttribute(float min, float max)
        {
            Min = Mathf.Min(min, max);
            Max = Mathf.Max(min, max);
        }
    }
}