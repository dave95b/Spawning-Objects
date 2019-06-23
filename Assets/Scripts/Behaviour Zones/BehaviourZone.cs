using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using Utilities;

namespace Core.BehaviourZones
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BehaviourZone : MonoBehaviour
    {
        [SerializeField]
        private Collider collider;

        [SerializeField]
        protected bool enter, exit;

        protected abstract Color GizmoColor { get; }


        public abstract void OnShapeEnter(Shape shape);
        public abstract void OnShapeExit(Shape shape);

        private void OnTriggerEnter(Collider other)
        {
            if (!enter)
                return;

            var shape = other.GetComponent<Shape>();
            if (shape != null)
                OnShapeEnter(shape);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!exit)
                return;

            var shape = other.GetComponent<Shape>();
            if (shape != null)
                OnShapeExit(shape);
        }


        protected virtual void Reset()
        {
            SetupCollider();
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }

        private void SetupCollider()
        {
            collider = GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;
        }

        private void OnDrawGizmos()
        {
            if (collider != null)
                GizmoDrawer.Draw(collider, GizmoColor);
        }
    }
}