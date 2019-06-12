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
                DoOnSpawned(shape);
        }

        public void OnSpawned(Shape spawned)
        {
            if (!isInSpawnZone)
                DoOnSpawned(spawned);
        }

        public void OnDesawnedInZone(Shape shape)
        {
            if (isInSpawnZone)
                DoOnDespawned(shape);
        }

        public void OnDespawned(Shape despawned)
        {
            if (!isInSpawnZone)
                DoOnDespawned(despawned);
        }

        protected abstract void DoOnSpawned(Shape spawned);
        protected abstract void DoOnDespawned(Shape despawned);

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