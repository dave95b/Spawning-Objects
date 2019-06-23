using UnityEngine;
using System.Collections.Generic;

namespace Core.BehaviourZones
{
    public abstract class CachedBehaviourZone<T> : BehaviourZone
    {
        protected Dictionary<Shape, T> cache = new Dictionary<Shape, T>();

        protected abstract T GetCachedData(Shape shape);


        protected override void OnShapeEnter(Shape shape)
        {
            cache[shape] = GetCachedData(shape);
        }

        protected override sealed void OnShapeExit(Shape shape)
        {
            if (!cache.TryGetValue(shape, out T data))
                return;

            OnShapeExit(shape, data);
            cache.Remove(shape);
        }

        protected abstract void OnShapeExit(Shape shape, T data);
    }
}