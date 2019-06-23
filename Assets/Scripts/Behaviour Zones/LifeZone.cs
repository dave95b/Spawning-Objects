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

        public override void OnShapeEnter(Shape shape) { }

        public override void OnShapeExit(Shape shape)
        {
            shape.Collider.enabled = false;
            killer.Kill(shape);
        }

        protected override void Reset()
        {
            base.Reset();
            exit = true;
        }
    }
}