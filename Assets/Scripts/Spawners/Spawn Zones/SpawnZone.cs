using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Zones
{
    public abstract class SpawnZone : SpawnPoint
    {
        protected abstract Vector3 Position { get; }

        public override sealed void Apply(Transform spawned)
        {
            spawned.transform.position = Position;
        }
    }
}
