using UnityEngine;
using System.Collections.Generic;

namespace Utilities
{
    public enum GizmoType
    {
        Shpere, Box
    }

    public static class GizmoDrawer
    {

        public static void Draw(Collider collider, Color color)
        {
            Gizmos.color = color;

            var transform = collider.transform;
            Vector3 scale = transform.lossyScale;

            if (collider is SphereCollider sphere)
            {
                scale = Vector3.one * Mathf.Max(scale.x, scale.y, scale.z);
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            }
            else if (collider is BoxCollider box)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
                Gizmos.DrawWireCube(box.center, box.size);
            }
            else
                Debug.LogError("Only Sphere and Box collider are supported");
        }

        public static void Draw(Transform transform, Color color, GizmoType gizmoType)
        {
            Gizmos.color = color;

            if (gizmoType == GizmoType.Shpere)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireSphere(Vector3.zero, 1f);
            }
            else
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }
    }
}