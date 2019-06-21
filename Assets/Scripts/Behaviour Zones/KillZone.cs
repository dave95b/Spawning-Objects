using UnityEngine;
using System.Collections;
using Utilities;
using UnityEngine.Assertions;

namespace Core.BehaviourZones
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField]
        private ShapeKiller killer;

        private void Awake()
        {
            var collider = GetComponent<Collider>();
            Assert.IsNotNull(collider);
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var shape = other.GetComponent<Shape>();
            if (shape != null)
            {
                shape.Collider.enabled = false;
                killer.Kill(shape);
            }
        }
    }
}