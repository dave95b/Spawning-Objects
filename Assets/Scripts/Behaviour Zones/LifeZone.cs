using UnityEngine;
using System.Collections;
using Utilities;

namespace Core.BehaviourZones
{
    public class LifeZone : BehaviourZone
    {
        [SerializeField]
        private ShapeKiller killer;

        protected override Color GizmoColor => Color.green;

        protected override void OnShapeEnter(Shape shape) { }

        protected override void OnShapeExit(Shape shape)
        {
            shape.Collider.enabled = false;
            killer.Kill(shape);
        }
    }
}