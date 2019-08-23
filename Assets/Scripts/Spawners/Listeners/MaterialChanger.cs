using UnityEngine;

namespace Core.Spawners.Listeners
{
    public class MaterialChanger : SpawnZoneListener
    {
        [SerializeField]
        private Material[] materials;

        protected override void OnShapeSpawned(Shape spawned)
        {
            for (int i = 0; i < spawned.Count; i++)
                spawned.SetMaterial(materials.GetRandom(), i);
        }

        protected override void OnShapeDespawned(Shape despawned) { }
    }
}