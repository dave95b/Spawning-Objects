using UnityEngine;
using System.Collections.Generic;

namespace SpawnerSystem.Spawners
{
    public class SpawnPoint : MonoBehaviour, ISpawnPoint
    {
        public virtual void Apply(Transform spawned)
        {
            spawned.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}
