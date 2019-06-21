using UnityEngine;
using System.Collections;
using NaughtyAttributes;

namespace Core.BehaviourZones
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BehaviourZone : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Collider collider;

        private void Awake()
        {
            if (collider != null)
                SetupCollider();
        }


        protected abstract void OnShapeEnter(Shape shape);
        protected abstract void OnShapeExit(Shape shape);

        private void OnTriggerEnter(Collider other)
        {
            var shape = other.GetComponent<Shape>();
            if (shape != null)
                OnShapeEnter(shape);
        }

        private void OnTriggerExit(Collider other)
        {
            var shape = other.GetComponent<Shape>();
            if (shape != null)
                OnShapeExit(shape);
        }


        private void Reset()
        {
            SetupCollider();
        }

        private void SetupCollider()
        {
            collider = GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;
        }
    }
}