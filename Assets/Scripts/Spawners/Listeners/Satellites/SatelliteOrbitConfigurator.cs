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
                float _frequency = orbitFrequency.Random;
                float _radius = radius.Random;
                Vector3 _orbitAxis = Random.onUnitSphere;

                var data = new SatelliteSystemData(_frequency, _radius, _orbitAxis, shape.transform);
                satelliteSystem.AddData(satellite.transform, data);

                Vector3 angularVelocity = -360f * _frequency * shape.transform.InverseTransformDirection(_orbitAxis) + Random.insideUnitSphere * 0.25f;
                rotationSystem.AddData(satellite.transform, angularVelocity);
            }
        }
    }
}