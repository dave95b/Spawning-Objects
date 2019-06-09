using UnityEngine;
using System.Collections.Generic;

namespace Core.Spawners.Zones
{
    public class ForwardVelocityApplier : MonoBehaviour, ISpawnZoneComponent
    {
        [SerializeField]
        private float minSpeed = 0.2f, maxSpeed = 2f;

        [SerializeField]
        private MovementDirection direction;

        public void Apply(Shape shape)
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

        enum MovementDirection
        {
            Forward, Outward, Random
        }
    }
}
