using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Systems;

namespace Core.Spawners.Listeners
{
    public class ForwardVelocityApplier : SpawnZoneListener
    {
        [SerializeField, FloatRangeSlider(0f, 5f)]
        private FloatRange speed;

        [SerializeField]
        private MovementDirection direction;

        [SerializeField]
        private MoveSystem moveSystem;


        protected override void DoOnSpawned(Shape spawned)
        {
            Vector3 velocity;
            if (direction == MovementDirection.Forward)
                velocity = transform.forward;
            else if (direction == MovementDirection.Outward)
                velocity = (spawned.transform.position - transform.position).normalized;
            else
                velocity = Random.onUnitSphere;

            float moveSpeed = speed.Random;
            if (moveSpeed != 0f)
                moveSystem.AddData(spawned.transform, velocity * moveSpeed);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            moveSystem.Remove(despawned.transform);
        }

        enum MovementDirection
        {
            Forward, Outward, Random
        }
    }
}
