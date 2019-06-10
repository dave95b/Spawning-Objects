using UnityEngine;
using System.Collections;
using SpawnerSystem.Spawners;
using NaughtyAttributes;
using Core.Spawners.Zones;

namespace Core.Spawners.Listeners
{
    public class ScaleRandomizer : MonoBehaviour, ISpawnListener<Shape>, ISpawnZoneComponent
    {
        [SerializeField, FloatRangeSlider(0.1f, 2f)]
        private FloatRange scale;

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
            spawned.transform.localScale = Vector3.one * scale.RandomRange;
        }

        public void OnDespawned(Shape despawned) { }
    }
}
