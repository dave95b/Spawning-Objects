using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using UnityEngine.Assertions;

namespace Core.Spawners.Zones
{
    public abstract class SpawnZone : SpawnPoint
    {
        protected abstract Vector3 Position { get; }

        private ISpawnZoneComponent[] components;

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
                component.Apply(shape);
        }
    }
}
