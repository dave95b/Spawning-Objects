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
        private NativeList<float3> planetPositions, satellitePositions;

        protected override void Awake()
        {
            base.Awake();
            planets = new TransformAccessArray(128, 12);
            planetPositions = new NativeList<float3>(128, Allocator.Persistent);
            satellitePositions = new NativeList<float3>(128, Allocator.Persistent);
        }
        
        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var planetPositionsJob = new PlanetPositionsJob
            {
                PlanetPositions = planetPositions
            };

            var satelliteJob = new SatelliteJob
            {
                Data = dataList,
                PlanetPositions = planetPositions,
                Time = Time.time,
                SatellitePositions = satellitePositions
            };

            var satellitePositionsJob = new SatellitePositionsJob
            {
                SatellitePositions = satellitePositions
            };

            inputHandle = planetPositionsJob.Schedule(planets, inputHandle);
            inputHandle = satelliteJob.Schedule(satellitePositions.Length, 256, inputHandle);
            inputHandle = satellitePositionsJob.Schedule(transforms, inputHandle);
        }

        public void AddData(Transform satellite, Transform planet, in SatelliteData data)
        {
            if (AddData(satellite, data))
            {
                planets.Add(planet);
                planetPositions.Add(float3.zero);
                satellitePositions.Add(float3.zero);
            }
        }

        protected override void OnRemove(int index)
        {
            Assert.AreEqual(planets.length, transformPositions.Count);

            planets.RemoveAtSwapBack(index);
            planetPositions.RemoveAtSwapBack(index);
            satellitePositions.RemoveAtSwapBack(index);
        }

        protected override void OnDestroy()
        {
            planets.Dispose();
            planetPositions.Dispose();
            satellitePositions.Dispose();
            base.OnDestroy();
        }
    }

    public readonly struct SatelliteData
    {
        public readonly float Frequency;
        public readonly float3 SinOffset, CosOffset;

        public SatelliteData(float frequency, float radius, Vector3 orbitAxis)
        {
            Frequency = frequency;
            SinOffset = new float3(0, 0, 1) * radius;

            CosOffset = math.cross(orbitAxis, UnityEngine.Random.onUnitSphere);
            CosOffset = math.normalize(CosOffset);
            CosOffset *= radius;
        }
    }

    [BurstCompile]
    struct PlanetPositionsJob : IJobParallelForTransform
    {
        [WriteOnly]
        public NativeArray<float3> PlanetPositions;

        public void Execute(int index, TransformAccess planet)
        {
            PlanetPositions[index] = planet.position;
        }
    }


    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct SatelliteJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<SatelliteData> Data;

        [ReadOnly]
        public NativeArray<float3> PlanetPositions;

        [ReadOnly]
        public float Time;

        [WriteOnly]
        public NativeArray<float3> SatellitePositions;

        public void Execute(int index)
        {
            var data = Data[index];
            float t = 2f * math.PI * data.Frequency * Time;

            SatellitePositions[index] = PlanetPositions[index] + data.CosOffset * math.cos(t) + data.SinOffset * math.sin(t);
        }
    }

    [BurstCompile]
    struct SatellitePositionsJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float3> SatellitePositions;

        public void Execute(int index, TransformAccess satellite)
        {
            satellite.position = SatellitePositions[index];
        }
    }
}