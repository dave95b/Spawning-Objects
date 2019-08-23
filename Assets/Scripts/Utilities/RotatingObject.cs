using UnityEngine;

namespace Utilities
{
    public class RotatingObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 angularVelocity;

        private void Update()
        {
            transform.Rotate(angularVelocity * Time.deltaTime);
        }
    }
}