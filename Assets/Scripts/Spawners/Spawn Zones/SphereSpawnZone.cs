using UnityEngine;
using Utilities;

namespace Core.Spawners.Zones
{
    public class SphereSpawnZone : SpawnZone
    {
        protected override Vector3 Position => transform.TransformPoint(Random.insideUnitSphere);

        private void OnDrawGizmos()
        {
            GizmoDrawer.Draw(transform, Color.cyan, GizmoType.Shpere);
        }
    }
}