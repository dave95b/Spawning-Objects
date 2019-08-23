using SpawnerSystem.ObjectPooling;
using SpawnerSystem.Spawners;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {
            Spawner.AddListener(new SpawnerInjector(Spawner));
        }
    }

    internal class SpawnerInjector : ISpawnListener<Shape>
    {
        private readonly Spawner<Shape> spawner;

        public SpawnerInjector(Spawner<Shape> spawner)
        {
            this.spawner = spawner;
        }

        public void OnSpawned(Shape spawned)
        {
            spawned.Spawner = spawner;
        }

        public void OnDespawned(Shape despawned) { }
    }
}