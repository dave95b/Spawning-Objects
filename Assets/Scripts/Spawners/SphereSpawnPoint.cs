using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using NaughtyAttributes;

namespace Core.Spawners
{
    public class SphereSpawnPoint : SpawnPoint
    {
        [SerializeField]
        private float radius;

        [SerializeField, MinValue(0.1f)]
        private float minScale;
        [SerializeField, MaxValue(1f)]
        private float maxScale = 1f;

        public override void Apply(Transform spawned)
        {
            Vector3 position = transform.TransformPoint(Random.insideUnitSphere);
            float scale = Random.Range(minScale, maxScale);

            spawned.SetPositionAndRotation(position, Random.rotation);
            spawned.localScale = Vector3.one * scale;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }
    }
}