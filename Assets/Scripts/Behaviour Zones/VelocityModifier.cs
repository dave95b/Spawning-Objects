using UnityEngine;
using System.Collections.Generic;
using Systems;

namespace Core.BehaviourZones
{
    public class VelocityModifier : BehaviourZone
    {
        [SerializeField]
        private MoveSystem system;

        [SerializeField, FloatRangeSlider(0f, 10f)]
        private FloatRange speed;

        protected override Color GizmoColor => Color.black;

        protected override void OnShapeEnter(Shape shape)
        {
            float _speed = speed.Random;
            if (_speed == 0f)
                system.Remove(shape.transform);
            else
                system.UpdateData(shape.transform, transform.forward * _speed);
        }

        protected override void OnShapeExit(Shape shape) { }
    }
}