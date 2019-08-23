using UnityEngine;
using Utilities;

namespace Core.Spawners.Zones
{
    public class CubeSpawnZone : SpawnZone
    {
        protected override Vector3 Position
        {
            get
            {
                Vector3 position = Vector3.zero;
                position.x = RandomPosition;
                position.y = RandomPosition;
                position.z = RandomPosition;

                return transform.TransformPoint(position);
            }
        }

        private float RandomPosition => Random.Range(-0.5f, 0.5f);

        private void OnDrawGizmos()
        {
            GizmoDrawer.Draw(transform, Color.cyan, GizmoType.Box);
        }
    }
}