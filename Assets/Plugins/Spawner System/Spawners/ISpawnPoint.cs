using UnityEngine;
using System.Collections.Generic;

namespace SpawnerSystem.Spawners
{
    public interface ISpawnPoint
    {
        void Apply<T>(T spawned) where T : Component;
    }
}