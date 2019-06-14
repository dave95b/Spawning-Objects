using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Systems;

namespace Core.Spawners.Listeners.Satellites
{
    public class SatelliteOrbitConfigurator : MonoBehaviour, ISatelliteConfigurator
    {
        [SerializeField, FloatRangeSlider(1f, 5f)]
        private FloatRange radius;

        [SerializeField, FloatRangeSlider(0.1f, 3f)]
        private FloatRange orbitFrequency;

        [SerializeField]
        private SatelliteSystem system;


        public void Configure(Shape shape, List<Shape> satellites)
        {
            foreach (var satellite in satellites)
            {
                var data = new SatelliteData(orbitFrequency.RandomRange, radius.RandomRange);
                system.AddData(satellite.transform, shape.transform, data);
            }
        }

        public void OnDespawned(Shape shape, List<Shape> satellites)
        {
            foreach (var satellite in satellites)
                system.Remove(satellite.transform);
        }
    }
}