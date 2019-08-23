using UnityEngine;

namespace Core.Spawners.Listeners
{
    public class ColliderController : SpawnZoneListener
    {
        [SerializeField]
        private bool control, status;

        protected override void OnShapeSpawned(Shape spawned)
        {
            if (control)
                spawned.Collider.enabled = status;
        }

        protected override void OnShapeDespawned(Shape despawned)
        {
            despawned.Collider.enabled = despawned.EnableCollider;
        }
    }
}