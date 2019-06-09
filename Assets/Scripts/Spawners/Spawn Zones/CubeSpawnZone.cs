using UnityEngine;
using System.Collections.Generic;

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
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}