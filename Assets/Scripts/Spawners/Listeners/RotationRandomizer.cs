using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Core.Spawners.Zones;
using Systems;

namespace Core.Spawners.Listeners
{
    public class RotationRandomizer : SpawnZoneListener
    {
        [SerializeField]
        private FloatRange speed;

        [SerializeField]
        private RotationSystem rotationSystem;

        protected override void DoOnSpawned(Shape spawned)
        {
            spawned.transform.rotation = Random.rotation;

            float rotateSpeed = speed.RandomRange;
            if (rotateSpeed != 0f)
                rotationSystem.AddData(spawned.transform, Random.onUnitSphere * speed.RandomRange);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            rotationSystem.Remove(despawned.transform);
        }
    }
}
