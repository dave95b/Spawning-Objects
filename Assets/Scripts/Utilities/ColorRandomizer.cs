using Core;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    [Serializable]
    public struct ColorRandomizer : IEquatable<ColorRandomizer>
    {
        public Color BaseColor;

        [FloatRangeSlider(0f, 1f)]
        public FloatRange HueRange, SaturationRange, ValueRange;

        public Color RandomColor
        {
            get
            {
                Color.RGBToHSV(BaseColor, out float hue, out float saturation, out float value);

                float randomHue = HueRange.Random;
                float randomSaturation = SaturationRange.Random;
                float randomValue = ValueRange.Random;

                return Color.HSVToRGB(
                    H: Random.Range(hue - randomHue, hue + randomHue),
                    S: Random.Range(saturation - randomSaturation, saturation + randomSaturation),
                    V: Random.Range(value - randomValue, value + randomValue));
            }
        }

        public ColorRandomizer(Color baseColor, FloatRange hueRange, FloatRange saturationRange, FloatRange valueRange)
        {
            BaseColor = baseColor;
            HueRange = hueRange;
            SaturationRange = saturationRange;
            ValueRange = valueRange;
        }

        public bool Equals(ColorRandomizer other)
        {
            return BaseColor.Equals(other.BaseColor)
                && HueRange.Equals(other.HueRange)
                && SaturationRange.Equals(other.SaturationRange)
                && ValueRange.Equals(other.ValueRange);
        }

        public override bool Equals(object obj)
        {
            if (obj is ColorRandomizer other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1165054906;
            hashCode = hashCode * -1521134295 + BaseColor.GetHashCode();
            hashCode = hashCode * -1521134295 + HueRange.GetHashCode();
            hashCode = hashCode * -1521134295 + SaturationRange.GetHashCode();
            hashCode = hashCode * -1521134295 + ValueRange.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ColorRandomizer left, ColorRandomizer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColorRandomizer left, ColorRandomizer right)
        {
            return !(left == right);
        }
    }
}