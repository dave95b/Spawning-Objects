using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Systems;

namespace Core.Spawners.Listeners
{
    public class ForwardVelocityApplier : SpawnZoneListener
    {
        [SerializeField]
        private float minSpeed = 0.2f, maxSpeed = 2f;

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

            moveSystem.AddData(spawned.transform, velocity);
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
