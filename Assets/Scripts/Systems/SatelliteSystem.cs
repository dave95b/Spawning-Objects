using UnityEngine;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Assertions;

namespace Systems
{
    public class SatelliteSystem : GameSystem<SatelliteData>
    {
        private TransformAccessArray planets;
        private NativeList<Vector3> planetPositions;

        protected override void Awake()
        {
            base.Awake();
            planets = new TransformAccessArray(128);
            planetPositions = new NativeList<Vector3>(128, Allocator.Persistent);
        }
        
        public override JobHandle OnUpdate(JobHandle inputHandle)
        {
            var positionsJob = new PlanetPositionsJob
            {
                PlanetPositions = planetPositions
            };

            var satelliteJob = new SatelliteJob
            {
                Data = dataList,
                PlanetPositions = planetPositions,
                Time = Time.time
            };

            inputHandle = positionsJob.Schedule(planets, inputHandle);
            return satelliteJob.Schedule(transforms, inputHandle);
        }

        public void AddData(Transform satellite, Transform planet, in SatelliteData data)
        {
            if (AddData(satellite, data))
            {
                planets.Add(planet);
                planetPositions.Add(Vector3.zero);
            }
        }

        protected override void OnRemove(int index)
        {
            Assert.AreEqual(planets.length, transformPositions.Count);

            planets.RemoveAtSwapBack(index);
            planetPositions.RemoveAtSwapBack(index);
        }

        protected override void OnDestroy()
        {
            planets.Dispose();
            planetPositions.Dispose();
            base.OnDestroy();
        }
    }

    public readonly struct SatelliteData
    {
        public readonly float Frequency;
        public readonly Vector3 SinOffset, CosOffset;

        public SatelliteData(float frequency, float radius, Vector3 orbitAxis)
        {
            Frequency = frequency;
            SinOffset = Vector3.forward * radius;

            do
            {
                CosOffset = Vector3.Cross(orbitAxis, UnityEngine.Random.onUnitSphere).normalized;
            }
            while (CosOffset.sqrMagnitude < 0.1f);

            CosOffset *= radius;
        }
    }

    [BurstCompile]
    struct PlanetPositionsJob : IJobParallelForTransform
    {
        [WriteOnly]
        public NativeArray<Vector3> PlanetPositions;

        public void Execute(int index, TransformAccess planet)
        {
            PlanetPositions[index] = planet.position;
        }
    }


    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct SatelliteJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<SatelliteData> Data;

        [ReadOnly]
        public NativeArray<Vector3> PlanetPositions;

        [ReadOnly]
        public float Time;

        public void Execute(int index, TransformAccess transform)
        {
            var data = Data[index];
            float t = 2f * math.PI * data.Frequency * Time;

            transform.position = PlanetPositions[index] + data.CosOffset * math.cos(t) + data.SinOffset * math.sin(t);
        }
    }
}