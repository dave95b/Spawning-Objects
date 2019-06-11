using UnityEngine;
using System.Collections;
using SpawnerSystem.Spawners;
using NaughtyAttributes;
using Core.Spawners.Zones;

namespace Core.Spawners.Listeners
{
    public class ScaleRandomizer : SpawnZoneListener
    {
        [SerializeField, FloatRangeSlider(0.1f, 2f)]
        private FloatRange scale;

        protected override void DoOnSpawned(Shape spawned)
        {
            spawned.transform.localScale = Vector3.one * scale.RandomRange;
        }

        protected override void DoOnDespawned(Shape despawned) { }
    }
}
