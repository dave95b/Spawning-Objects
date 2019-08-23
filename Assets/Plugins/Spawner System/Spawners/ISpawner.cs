using UnityEngine;

namespace SpawnerSystem.Spawners
{
    public interface ISpawner<T> where T : Component
    {
        T Spawn();
        T Spawn(ISpawnPoint spawnPoint);
        void SpawnMany(T[] spawnedArray);
        void SpawnMany(T[] spawnedArray, int count);
        void SpawnMany(T[] spawnedArray, ISpawnPoint spawnPoint);
        void SpawnMany(T[] spawnedArray, int count, ISpawnPoint spawnPoint);
        void Despawn(T spawned);
    }
}
