using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using NaughtyAttributes;

namespace Core.Spawners.Zones
{
    public class SphereSpawnZone : SpawnZone
    {
        protected override Vector3 Position => transform.TransformPoint(Random.insideUnitSphere);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }
    }
}