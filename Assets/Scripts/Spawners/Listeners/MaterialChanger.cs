using UnityEngine;
using System.Collections;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Listeners
{
    public class MaterialChanger : MonoBehaviour, ISpawnListener<Shape>
    {
        [SerializeField]
        private Material[] materials;

        public void OnSpawned(Shape spawned)
        {
            spawned.Material = materials.GetRandom();
        }

        public void OnDespawned(Shape despawned) { }
    }
}