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

        private List<ISpawnListener<Shape>> spawnListeners;
        protected override List<ISpawnListener<Shape>> SpawnListeners => spawnListeners;


        private void Awake()
        {
            var listeners = GetComponentsInChildren<ISpawnListener<Shape>>();
            spawnListeners = new List<ISpawnListener<Shape>>(listeners);
        }
    }
}