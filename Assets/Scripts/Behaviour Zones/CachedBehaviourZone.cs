using System.Collections.Generic;

namespace Core.BehaviourZones
{
    public abstract class CachedBehaviourZone<T> : BehaviourZone
    {
        protected Dictionary<Shape, T> cache = new Dictionary<Shape, T>();

        protected abstract T GetCachedData(Shape shape);


        public override sealed void OnShapeEnter(Shape shape)
        {
            var data = GetCachedData(shape);
            cache[shape] = data;
            OnShapeEnter(shape, data);
        }

        public override sealed void OnShapeExit(Shape shape)
        {
            if (!cache.TryGetValue(shape, out T data))
                data = GetCachedData(shape);

            OnShapeExit(shape, data);
            cache.Remove(shape);
        }

        protected abstract void OnShapeEnter(Shape shape, T data);
        protected abstract void OnShapeExit(Shape shape, T data);
    }
}