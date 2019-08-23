using UnityEngine;

namespace SpawnerSystem.Spawners
{
    public interface ISpawnPoint
    {
        void Apply<T>(T spawned) where T : Component;
    }
}