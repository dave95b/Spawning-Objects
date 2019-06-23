using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using Utilities;

namespace Core.BehaviourZones
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class BehaviourZone : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Collider collider;

        [SerializeField]
        protected bool enter, exit;

        protected abstract Color GizmoColor { get; }

        private void Awake()
        {
            if (collider != null)
                SetupCollider();
        }


        protected abstract void OnShapeEnter(Shape shape);
        protected abstract void OnShapeExit(Shape shape);

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