using UnityEngine;
using System.Collections;
using SpawnerSystem.Spawners;
using NaughtyAttributes;

namespace Core.Spawners.Listeners
{
    public class ScaleRandomizer : MonoBehaviour, ISpawnListener<Shape>
    {
        [SerializeField, MinValue(0.1f)]
        private float minScale;
        [SerializeField, MaxValue(1f)]
        private float maxScale = 1f;

        public void OnSpawned(Shape spawned)
        {
            float scale = Random.Range(minScale, maxScale);
            spawned.transform.localScale = Vector3.one * scale;
        }

        public void OnDespawned(Shape despawned) { }
    }
}
