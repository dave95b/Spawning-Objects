using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Zones
{
    public class ForwardVelocityApplier : MonoBehaviour, ISpawnZoneComponent, ISpawnListener<Shape>
    {
        [SerializeField]
        private float minSpeed = 0.2f, maxSpeed = 2f;

        [SerializeField]
        private MovementDirection direction;


        public void Apply(Shape shape)
        {
            DoApply(shape);
        }
        
        public void OnSpawned(Shape spawned)
        {
            DoApply(spawned);
        }

        private void DoApply(Shape shape)
        {
            Vector3 velocity;
            if (direction == MovementDirection.Forward)
                velocity = transform.forward;
            else if (direction == MovementDirection.Outward)
                velocity = (shape.transform.position - transform.position).normalized;
            else
                velocity = Random.onUnitSphere;

            shape.Velocity = velocity * Random.Range(minSpeed, maxSpeed);
        }

        public void OnDespawned(Shape despawned) { }


        enum MovementDirection
        {
            Forward, Outward, Random
        }
    }
}
