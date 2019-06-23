using UnityEngine;
using System.Collections.Generic;
using Systems;
using NaughtyAttributes;
using System;

namespace Core.BehaviourZones
{
    enum Direction
    {
        Unchanged, Reverse, Forward
    }

    public class VelocityZone : CachedBehaviourZone<Vector3>
    {
        [SerializeField]
        private MoveSystem system;

        [SerializeField, FloatRangeSlider(0f, 10f), HideIf("useMultiplier")]
        private FloatRange speed;

        [SerializeField, FloatRangeSlider(0f, 10f), ShowIf("useMultiplier")]
        private FloatRange multiplier;

        [SerializeField]
        private bool useMultiplier;

        [SerializeField]
        private Direction enterDirection, exitDirection;

        protected override Color GizmoColor => Color.black;

        private Dictionary<Direction, Func<Vector3, Vector3>> solvers = new Dictionary<Direction, Func<Vector3, Vector3>>();


        private void Awake()
        {
            solvers[Direction.Unchanged] = (velocity) => velocity;
            solvers[Direction.Reverse] = (velocity) => -velocity;
            solvers[Direction.Forward] = (velocity) => transform.forward * velocity.magnitude;
        }


        protected override Vector3 GetCachedData(Shape shape)
        {
            if (system.TryGetData(shape.transform, out Vector3 velocity))
                return velocity;

            return Vector3.zero;
        }

        protected override void OnShapeEnter(Shape shape, Vector3 initialVelocity)
        {
            Vector3 velocity = CalculateVelocity(initialVelocity, enterDirection);
            if (velocity == Vector3.zero)
                system.Remove(shape.transform);
            else
                system.UpdateData(shape.transform, velocity);
        }

        protected override void OnShapeExit(Shape shape, Vector3 data)
        {
            Vector3 velocity = solvers[exitDirection](data);
            system.UpdateData(shape.transform, velocity);
        }

        private Vector3 CalculateVelocity(Vector3 initialVelocity, Direction direction)
        {
            Vector3 velocity = solvers[direction](initialVelocity);

            if (useMultiplier)
                return velocity * multiplier.Random;
            else
                return velocity * speed.Random;
        }

        protected override void Reset()
        {
            base.Reset();
            enter = true;
        }
    }
}