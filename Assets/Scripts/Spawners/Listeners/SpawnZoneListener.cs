using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using Core.Spawners.Zones;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Listeners
{
    public abstract class SpawnZoneListener : MonoBehaviour, ISpawnZoneComponent, ISpawnListener<Shape>
    {
        [SerializeField, ReadOnly]
        private bool isInSpawnZone;

        public void OnSpawnedInZone(Shape shape)
        {
            if (isInSpawnZone)
                OnShapeSpawned(shape);
        }

        public void OnSpawned(Shape spawned)
        {
            if (!isInSpawnZone)
                OnShapeSpawned(spawned);
        }

        public void OnDesawnedInZone(Shape shape)
        {
            if (isInSpawnZone)
                OnShapeDespawned(shape);
        }

        public void OnDespawned(Shape despawned)
        {
            if (!isInSpawnZone)
                OnShapeDespawned(despawned);
        }

        protected abstract void OnShapeSpawned(Shape spawned);
        protected abstract void OnShapeDespawned(Shape despawned);

        private void Awake()
        {
            isInSpawnZone = GetComponent<SpawnZone>() != null;
        }

        private void Reset()
        {
            isInSpawnZone = GetComponent<SpawnZone>() != null;
        }
    }
}