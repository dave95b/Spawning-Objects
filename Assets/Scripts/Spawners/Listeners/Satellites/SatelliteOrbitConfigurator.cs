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
        private SatelliteSystem satelliteSystem;

        [SerializeField]
        private RotationSystem rotationSystem;


        public void Configure(Shape shape, List<Shape> satellites)
        {
            foreach (var satellite in satellites)
            {
                float _frequency = orbitFrequency.RandomRange;
                float _radius = radius.RandomRange;
                Vector3 _orbitAxis = Random.onUnitSphere;

                var data = new SatelliteData(_frequency, _radius, _orbitAxis);
                satelliteSystem.AddData(satellite.transform, shape.transform, data);

                Vector3 angularVelocity = -360f * _frequency * shape.transform.InverseTransformDirection(_orbitAxis) + Random.insideUnitSphere * 0.25f;
                rotationSystem.AddData(satellite.transform, angularVelocity);
            }
        }

        public void OnDespawned(Shape shape, List<Shape> satellites)
        {
            foreach (var satellite in satellites)
            {
                satelliteSystem.Remove(satellite.transform);
                rotationSystem.Remove(satellite.transform);
            }
        }
    }
}