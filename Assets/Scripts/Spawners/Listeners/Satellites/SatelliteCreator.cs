using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Systems;
using System.Collections;
using Utilities;

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
        private FloatRange growDuration, shrinkDuration;

        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        [SerializeField]
        private MoveSystem moveSystem;

        [SerializeField]
        private SatelliteSystem satelliteSystem;

        [SerializeField]
        private ScaleSystem scaleSystem;

        private Spawner<Shape> Spawner => spawnerPreparer.Spawner;

        private Dictionary<Shape, List<Shape>> satellites = new Dictionary<Shape, List<Shape>>();
        private ISatelliteConfigurator[] configurators;

        private ActionSource<Shape> spawnedActionSource, despawnedActionSource;

        private void Awake()
        {
            configurators = GetComponentsInChildren<ISatelliteConfigurator>();
            spawnedActionSource = new ActionSource<Shape>((shape) => () => scaleSystem.Remove(shape.transform));
            despawnedActionSource = new ActionSource<Shape>((satellite) => () => OnSatelliteDespawned(satellite));
        }


        protected override void DoOnSpawned(Shape spawned)
        {
            if (!satellites.TryGetValue(spawned, out var list))
            {
                list = new List<Shape>(5);
                satellites[spawned] = list;
            }

            int count = (int)amount.Random;

            for (int i = 0; i < count; i++)
            {
                Shape satellite = SpawnSatelliteFor(spawned);
                list.Add(satellite);
                scaleSystem.AddData(satellite.transform, growDuration.Random, 0f, satellite.Scale, spawnedActionSource[satellite]);
            }

            foreach (var configurator in configurators)
                configurator.Configure(spawned, list);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            var shapeSatellites = satellites[despawned];

            foreach (var satellite in shapeSatellites)
            {
                var transform = satellite.transform;
                
                scaleSystem.Remove(transform);
                scaleSystem.AddData(transform, shrinkDuration.Random, transform.localScale.x, endScale: 0f, despawnedActionSource[satellite]);

                StartCoroutine(EjectFromOrbit(satellite));
            }

            shapeSatellites.Clear();
        }

        private void OnSatelliteDespawned(Shape satellite)
        {
            foreach (var configurator in configurators)
                configurator.OnDespawned(satellite);

            scaleSystem.Remove(satellite.transform);
            moveSystem.Remove(satellite.transform);
            Spawner.Despawn(satellite);
        }


        private Shape SpawnSatelliteFor(Shape shape)
        {
            Shape satellite = Spawner.Spawn();
            satellite.transform.position = shape.transform.position + Random.onUnitSphere * distance.Random;
            satellite.transform.localScale = Vector3.zero;
            satellite.Scale = shape.Scale * scale.Random;

            return satellite;
        }

        private IEnumerator EjectFromOrbit(Shape satellite)
        {
            Vector3 position = satellite.transform.localPosition;
            yield return null;
            Vector3 velocity = (satellite.transform.localPosition - position) / Time.deltaTime;
            satelliteSystem.Remove(satellite.transform);

            moveSystem.AddData(satellite.transform, velocity);
        }
    }
}