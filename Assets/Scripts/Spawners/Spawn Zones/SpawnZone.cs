using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using UnityEngine.Assertions;

namespace Core.Spawners.Zones
{
    public abstract class SpawnZone : SpawnPoint, ISpawnListener<Shape>
    {
        protected abstract Vector3 Position { get; }

        private ISpawnZoneComponent[] components;
        private HashSet<Shape> spawnedShapes = new HashSet<Shape>();

        private void Awake()
        {
            components = GetComponentsInChildren<ISpawnZoneComponent>();
        }

        public override sealed void Apply<T>(T spawned)
        {
            spawned.transform.position = Position;

            Shape shape = spawned as Shape;
            Assert.IsNotNull(shape);

            foreach (var component in components)
                component.OnSpawnedInZone(shape);

            spawnedShapes.Add(shape);
        }

        public void OnDespawned(Shape despawned)
        {
            bool removed = spawnedShapes.Remove(despawned);
            if (removed)
            {
                foreach (var component in components)
                    component.OnDesawnedInZone(despawned);
            }
        }

        public void OnSpawned(Shape spawned) { }
    }
}
