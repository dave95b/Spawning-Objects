using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Listeners
{
    public class RotationRandomizer : MonoBehaviour, ISpawnListener<Shape>
    {
        [SerializeField]
        private float minSpeed = 10, maxSpeed = 90;

        public void OnSpawned(Shape spawned)
        {
            spawned.transform.rotation = Random.rotation;
            spawned.AngularVelocity = Random.onUnitSphere * Random.Range(minSpeed, maxSpeed);
        }

        public void OnDespawned(Shape despawned) { }
    }
}
