using UnityEngine;
using Systems;

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

        protected override void DoOnSpawned(Shape spawned)
        {
            Vector3 oscillationOffset;
            if (direction == OffsetDirection.Outward)
                oscillationOffset = (spawned.transform.position - transform.position).normalized;
            else
                oscillationOffset = Random.onUnitSphere;

            var data = new OscillationData(oscillationOffset * offset.Random, frequency.Random);

            system.AddData(spawned.transform, data);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            system.Remove(despawned.transform);
        }
    }

    enum OffsetDirection
    {
        Outward, Random
    }
}