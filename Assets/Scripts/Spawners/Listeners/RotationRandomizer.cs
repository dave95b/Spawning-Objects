using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Listeners
{
    public class RotationRandomizer : MonoBehaviour, ISpawnListener<Shape>
    {
        public void OnSpawned(Shape spawned)
        {
            spawned.transform.rotation = Random.rotation;
        }

        public void OnDespawned(Shape despawned) { }
    }
}
