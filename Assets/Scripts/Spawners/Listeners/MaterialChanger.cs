using UnityEngine;
using System.Collections;
using SpawnerSystem.Spawners;
using Core.Spawners.Zones;

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