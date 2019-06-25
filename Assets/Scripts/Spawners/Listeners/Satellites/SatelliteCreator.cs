using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Systems;
using System.Collections;
using Utilities;
using UnityEngine.Assertions;

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

        [SerializeField, FloatRangeSlider(0.1f, 2f)]
        private FloatRange growDuration;

        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        [SerializeField]
        private SatelliteSystem satelliteSystem;

        [SerializeField]
        private ScaleSystem scaleSystem;

        [SerializeField]
        private ShapeKiller killer;

        private Spawner<Shape> Spawner => spawnerPreparer.Spawner;

        private Dictionary<Shape, List<Shape>> satellites = new Dictionary<Shape, List<Shape>>();

        private ISatelliteConfigurator[] configurators;

        private ActionSource<Shape> spawnedActionSource;

        private void Awake()
        {
            configurators = GetComponentsInChildren<ISatelliteConfigurator>();
            spawnedActionSource = new ActionSource<Shape>((shape) => () => scaleSystem.Remove(shape.transform));
        }

        protected override void OnShapeSpawned(Shape spawned)
        {
            int count = (int)amount.Random;
            if (count == 0)
                return;

            List<Shape> list = GetOrCreateSatelliteList(spawned);
            SpawnSatellites(spawned, count, list);

            foreach (var configurator in configurators)
                configurator.Configure(spawned, list);
        }

        private void SpawnSatellites(Shape planet, int count, List<Shape> list)
        {
            for (int i = 0; i < count; i++)
            {
                Shape satellite = SpawnSatelliteFor(planet);
                list.Add(satellite);

                var data = new ScaleSystemData(growDuration.Random, startScale: 0f, satellite.Scale, spawnedActionSource[satellite]);
                scaleSystem.AddData(satellite.transform, data);
            }
        }

        private List<Shape> GetOrCreateSatelliteList(Shape spawned)
        {
            if (!satellites.TryGetValue(spawned, out var list))
            {
                list = new List<Shape>(5);
                satellites[spawned] = list;
            }

            return list;
        }


        protected override void OnShapeDespawned(Shape despawned)
        {
            if (!satellites.TryGetValue(despawned, out var shapeSatellites))
                return;

            KillSatellites(shapeSatellites);
        }

        private void KillSatellites(List<Shape> shapeSatellites)
        {
            foreach (var satellite in shapeSatellites)
                killer.Kill(satellite);

            shapeSatellites.Clear();
        }

        private Shape SpawnSatelliteFor(Shape shape)
        {
            Shape satellite = Spawner.Spawn();
            satellite.transform.position = shape.transform.position + Random.onUnitSphere * distance.Random;
            satellite.transform.localScale = Vector3.zero;
            satellite.Scale = shape.Scale * scale.Random;

            return satellite;
        }
    }
}