using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using SpawnerSystem.ObjectPooling;

namespace Core.Spawners
{
    public class ShapeSpawnerPreparer : SpawnerPreparer<Shape>
    {
        [SerializeField]
        private ShapeMultiPoolPreparer preparer;
        protected override MultiPoolPreparer<Shape> PoolPreparer => preparer;

        protected override List<ISpawnListener<Shape>> SpawnListeners
        {
            get
            {
                var listeners = GetComponentsInChildren<ISpawnListener<Shape>>();
                return new List<ISpawnListener<Shape>>(listeners);
            }
        }
    }
}