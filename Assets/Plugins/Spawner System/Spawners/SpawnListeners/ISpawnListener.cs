using UnityEngine;

namespace SpawnerSystem.Spawners
{
    public interface ISpawnListener<T> where T : Component
    {
        void OnSpawned(T spawned);
        void OnDespawned(T despawned);
    }
}