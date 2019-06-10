using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Core.Spawners.Zones;

namespace Core.Spawners.Listeners
{
    public class RotationRandomizer : MonoBehaviour, ISpawnListener<Shape>, ISpawnZoneComponent
    {
        [SerializeField, FloatRangeSlider(0, 180)]
        private FloatRange speed;

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
            spawned.transform.rotation = Random.rotation;
            spawned.AngularVelocity = Random.onUnitSphere * speed.RandomRange;
        }

        public void OnDespawned(Shape despawned) { }
    }
}
