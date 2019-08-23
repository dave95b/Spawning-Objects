using UnityEngine;
using Utilities;

namespace Core.BehaviourZones
{
    public class KillZone : BehaviourZone
    {
        [SerializeField]
        private ShapeKiller killer;

        protected override Color GizmoColor => Color.red;

        public override void OnShapeEnter(Shape shape)
        {
            shape.Collider.enabled = false;
            killer.Kill(shape);
        }

        public override void OnShapeExit(Shape shape) { }

        protected override void Reset()
        {
            base.Reset();
            enter = true;
        }
    }
}