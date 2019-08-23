using NaughtyAttributes;
using UnityEngine;

namespace Core.BehaviourZones
{
    public class ProbabilityZone : BehaviourZone
    {
        [SerializeField]
        private BehaviourZone target;

        [SerializeField, MinValue(0f), MaxValue(1f)]
        private float probability;

        protected override Color GizmoColor => Color.clear;

        public override void OnShapeEnter(Shape shape)
        {
            if (Random.value < probability)
                target.OnShapeEnter(shape);
        }

        public override void OnShapeExit(Shape shape)
        {
            if (Random.value < probability)
                target.OnShapeExit(shape);
        }
    }
}