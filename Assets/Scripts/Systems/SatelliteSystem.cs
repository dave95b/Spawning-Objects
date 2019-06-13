using UnityEngine;
using System.Collections.Generic;
using Core;
using Unity.Collections;

namespace Systems
{
    public class SatelliteSystem : MonoBehaviour
    {
        private List<SatelliteData> satellites = new List<SatelliteData>();
        protected Dictionary<Transform, int> transformPositions = new Dictionary<Transform, int>(new TransformComparer());

        void Update()
        {
            int count = satellites.Count;
            float time = Time.time;

            for (int i = 0; i < count; i++)
            {
                var data = satellites[i];
                float t = 2f * Mathf.PI * data.Frequency * time;

                Vector3 position = data.Planet.position + data.CosOffset * Mathf.Cos(t) + data.SinOffset * Mathf.Sin(t);
                data.Satellite.position = position;
            }
        }

        public void AddData(in SatelliteData data)
        {
            if (transformPositions.ContainsKey(data.Satellite))
                return;

            int index = satellites.Count;
            transformPositions[data.Satellite] = index;

            satellites.Add(data);
        }

        public void Remove(Transform transform)
        {
            if (!transformPositions.ContainsKey(transform))
                return;

            int index = transformPositions[transform];

            if (satellites.Count > 0)
            {
                Transform last = satellites[satellites.Count - 1].Satellite;
                transformPositions[last] = index;
            }

            satellites.RemoveAtSwapBack(index);

            transformPositions.Remove(transform);
        }
    }

    public readonly struct SatelliteData
    {
        public readonly Transform Satellite, Planet;
        public readonly float Frequency;
        public readonly Vector3 SinOffset, CosOffset;

        public SatelliteData(Transform satellite, Transform planet, float frequency, float radius) 
        {
            Satellite = satellite;
            Planet = planet;
            Frequency = frequency;

            SinOffset = Vector3.forward * radius;

            Vector3 orbitAxis = Random.onUnitSphere;
            do
            {
                CosOffset = Vector3.Cross(orbitAxis, Random.onUnitSphere).normalized;
            }
            while (CosOffset.sqrMagnitude < 0.1f);

            CosOffset *= radius;
        }
    }
}