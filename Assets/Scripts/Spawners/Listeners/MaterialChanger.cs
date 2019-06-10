using UnityEngine;
using System.Collections;
using SpawnerSystem.Spawners;
using Core.Spawners.Zones;

namespace Core.Spawners.Listeners
{
    public class MaterialChanger : MonoBehaviour, ISpawnListener<Shape>, ISpawnZoneComponent
    {
        [SerializeField]
        private Material[] materials;

        public void Apply(Shape shape)
        {
            DoApply(shape);
        }
        public void OnSpawned(Shape spawned)
        {
            DoApply(spawned);
        }

        private void DoApply(Shape spawned)
        {
            for (int i = 0; i < spawned.Count; i++)
                spawned.SetMaterial(materials.GetRandom(), i);
        }

        public void OnDespawned(Shape despawned) { }
    }
}