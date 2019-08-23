using SpawnerSystem.ObjectPooling;
using UnityEngine;

namespace Core.Spawners
{
    public class ShapePoolPreparer : PoolPreparer<Shape>
    {
        [SerializeField]
        private ShapePoolable prefab;
        protected override Poolable<Shape> Prefab => prefab;
    }
}