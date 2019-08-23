using Systems;
using UnityEngine;

namespace Core.Spawners.Listeners
{
    public class OscillationRandomizer : SpawnZoneListener
    {
        [SerializeField, FloatRangeSlider(-2f, 2f)]
        private FloatRange offset;

        [SerializeField, FloatRangeSlider(0.05f, 1)]
        private FloatRange frequency;

        [SerializeField]
        private OffsetDirection direction;

        [SerializeField]
        private OscillationSystem system;

        protected override void OnShapeSpawned(Shape spawned)
        {
            Vector3 oscillationOffset;
            if (direction == OffsetDirection.Outward)
                oscillationOffset = (spawned.transform.position - transform.position).normalized;
            else
                oscillationOffset = Random.onUnitSphere;

            var data = new OscillationData(oscillationOffset * offset.Random, frequency.Random);

            if (data.Offset != Vector3.zero)
                system.AddData(spawned.transform, data);
        }

        protected override void OnShapeDespawned(Shape despawned)
        {
            system.Remove(despawned.transform);
        }
    }

    internal enum OffsetDirection
    {
        Outward, Random
    }
}