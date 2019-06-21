using UnityEngine;
using System.Collections;
using Utilities;
using UnityEngine.Assertions;

namespace Core.BehaviourZones
{
    public class KillZone : BehaviourZone
    {
        [SerializeField]
        private ShapeKiller killer;

        protected override Color GizmoColor => Color.red;

        protected override void OnShapeEnter(Shape shape)
        {
            shape.Collider.enabled = false;
            killer.Kill(shape);
        }

        protected override void OnShapeExit(Shape shape) { }
    }
}