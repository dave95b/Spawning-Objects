using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.ObjectPooling;

namespace Core.Spawners
{
    public class ShapePoolPreparer : PoolPreparer<Shape>
    {
        [SerializeField]
        private ShapePoolable prefab;
        protected override Poolable<Shape> Prefab => prefab;
    }
}