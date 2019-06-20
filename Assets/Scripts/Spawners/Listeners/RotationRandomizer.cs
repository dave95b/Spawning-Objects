﻿using Systems;
using UnityEngine;

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

            float rotateSpeed = speed.Random;
            if (rotateSpeed != 0f)
                rotationSystem.AddData(spawned.transform, Random.onUnitSphere * speed.Random);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            rotationSystem.Remove(despawned.transform);
        }
    }
}
