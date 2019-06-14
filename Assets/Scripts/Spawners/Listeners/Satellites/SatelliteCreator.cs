using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Listeners.Satellites
{
    public class SatelliteCreator : SpawnZoneListener
    {
        [SerializeField, FloatRangeSlider(0f, 5f)]
        private FloatRange amount;

        [SerializeField, FloatRangeSlider(0.5f, 3f)]
        private FloatRange distance;

        [SerializeField, FloatRangeSlider(0.1f, 0.75f)]
        private FloatRange scale;

        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        private Spawner<Shape> Spawner => spawnerPreparer.Spawner;

        private Dictionary<Shape, List<Shape>> satellites = new Dictionary<Shape, List<Shape>>();
        private ISatelliteConfigurator[] configurators;

        private void Awake()
        {
            configurators = GetComponentsInChildren<ISatelliteConfigurator>();
        }


        protected override void DoOnSpawned(Shape spawned)
        {
            if (!satellites.TryGetValue(spawned, out var list))
            {
                list = new List<Shape>(5);
                satellites[spawned] = list;
            }

            int count = (int)amount.RandomRange;

            for (int i = 0; i < count; i++)
            {
                Shape satellite = SpawnSatelliteFor(spawned);
                list.Add(satellite);
            }

            foreach (var configurator in configurators)
                configurator.Configure(spawned, list);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            var shapeSatellites = satellites[despawned];

            foreach (var configurator in configurators)
                configurator.OnDespawned(despawned, shapeSatellites);

            foreach (var satellite in shapeSatellites)
                Spawner.Despawn(satellite);

            shapeSatellites.Clear();
        }


        private Shape SpawnSatelliteFor(Shape shape)
        {
            Shape satellite = Spawner.Spawn();
            satellite.transform.position = shape.transform.position + Random.onUnitSphere * distance.RandomRange;
            satellite.transform.localScale = shape.transform.localScale * scale.RandomRange;

            return satellite;
        }
    }
}