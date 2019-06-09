using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using NaughtyAttributes;

namespace Core.Spawners
{
    public class SphereSpawnPoint : SpawnPoint
    {
        public override void Apply(Transform spawned)
        {
            Vector3 position = transform.TransformPoint(Random.insideUnitSphere);
            spawned.transform.position = position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }
    }
}