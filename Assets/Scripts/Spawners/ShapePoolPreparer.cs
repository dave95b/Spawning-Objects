using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.ObjectPooling;

namespace Core
{
    public class ShapePoolPreparer : PoolPreparer<Shape>
    {
        [SerializeField]
        private ShapePoolable prefab;
        protected override Poolable<Shape> Prefab => prefab;
    }
}