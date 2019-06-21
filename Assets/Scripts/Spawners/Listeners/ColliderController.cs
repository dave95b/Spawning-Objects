using UnityEngine;
using System.Collections.Generic;

namespace Core.Spawners.Listeners
{
    public class ColliderController : SpawnZoneListener
    {
        [SerializeField]
        private bool control, status;

        protected override void DoOnSpawned(Shape spawned)
        {
            if (control)
                spawned.Collider.enabled = status;
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            despawned.Collider.enabled = despawned.EnableCollider;
        }
    }
}