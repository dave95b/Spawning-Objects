using UnityEngine;
using System.Collections.Generic;

namespace SpawnerSystem.Spawners
{
    public class SpawnPoint : MonoBehaviour, ISpawnPoint
    {
        public virtual void Apply<T>(T spawned) where T : Component
        {
            spawned.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}
