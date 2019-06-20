using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core
{
    [Serializable]
    public struct FloatRange : IEquatable<FloatRange>
    {
        [SerializeField]
        private float min, max;

        public float Min
        {
            get => min;
            set => min = Mathf.Min(value, max);
        }

        public float Max
        {
            get => max;
            set => max = Mathf.Max(value, min);
        }

        public float Random => UnityEngine.Random.Range(min, max);


        public bool Equals(FloatRange other)
        {
            return min == other.min && max == other.max;
        }

        public override bool Equals(object obj)
        {
            if (obj is FloatRange other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -897720056;
            hashCode = hashCode * -1521134295 + min.GetHashCode();
            hashCode = hashCode * -1521134295 + max.GetHashCode();
            return hashCode;
        }
    }
}