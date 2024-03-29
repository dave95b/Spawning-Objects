﻿using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Jobs;

namespace Systems
{
    public class SatelliteSystem : GameSystem<SatelliteSystemData>
    {
        private TransformAccessArray planets;
        private NativeList<SatelliteData> data;
        private NativeList<float3> planetPositions, satellitePositions;

        protected override void Awake()
        {
            base.Awake();
            planets = new TransformAccessArray(128, 12);
            data = new NativeList<SatelliteData>(128, Allocator.Persistent);
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
                Data = data,
                PlanetPositions = planetPositions,
                Time = Time.time,
                SatellitePositions = satellitePositions
            };

            var satellitePositionsJob = new SatellitePositionsJob
            {
                SatellitePositions = satellitePositions
            };

            inputHandle = planetPositionsJob.Schedule(planets, inputHandle);
            inputHandle = satelliteJob.Schedule(data.Length, 256, inputHandle);
            inputHandle = satellitePositionsJob.Schedule(transforms, inputHandle);
        }

        public override SatelliteSystemData GetData(int index)
        {
            var satelliteData = data[index];
            float radius = math.length(satelliteData.SinOffset);
            return new SatelliteSystemData(satelliteData.Frequency, radius, satelliteData.CosOffset, planets[index]);
        }

        protected override void OnAdd(SatelliteSystemData systemData)
        {
            var satelliteData = new SatelliteData(systemData);
            data.Add(satelliteData);
            planets.Add(systemData.Planet);
            planetPositions.Add(float3.zero);
            satellitePositions.Add(float3.zero);
        }

        protected override void OnRemove(int index)
        {
            Assert.AreEqual(planets.length, transformPositions.Count);

            data.RemoveAtSwapBack(index);
            planets.RemoveAtSwapBack(index);
            planetPositions.RemoveAtSwapBack(index);
            satellitePositions.RemoveAtSwapBack(index);
        }

        protected override void OnUpdateData(int index, SatelliteSystemData systemData)
        {
            var satelliteData = new SatelliteData(systemData);
            data[index] = satelliteData;
            planets[index] = systemData.Planet;
            planetPositions[index] = systemData.Planet.position;
        }

        protected override void OnDestroy()
        {
            data.Dispose();
            planets.Dispose();
            planetPositions.Dispose();
            satellitePositions.Dispose();
            base.OnDestroy();
        }
    }

    public readonly struct SatelliteSystemData
    {
        public readonly float Frequency, Radius;
        public readonly Vector3 OrbitAxis;
        public readonly Transform Planet;

        public SatelliteSystemData(float frequency, float radius, Vector3 orbitAxis, Transform planet)
        {
            Frequency = frequency;
            Radius = radius;
            OrbitAxis = orbitAxis;
            Planet = planet;
        }
    }

    internal readonly struct SatelliteData
    {
        public readonly float Frequency;
        public readonly float3 SinOffset, CosOffset;

        public SatelliteData(in SatelliteSystemData data)
        {
            Frequency = data.Frequency;
            SinOffset = new float3(0, 0, 1) * data.Radius;
            CosOffset = data.OrbitAxis * data.Radius;
        }
    }

    [BurstCompile]
    internal struct PlanetPositionsJob : IJobParallelForTransform
    {
        [WriteOnly]
        public NativeArray<float3> PlanetPositions;

        public void Execute(int index, TransformAccess planet)
        {
            PlanetPositions[index] = planet.position;
        }
    }

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct SatelliteJob : IJobParallelFor
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
    internal struct SatellitePositionsJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float3> SatellitePositions;

        public void Execute(int index, TransformAccess satellite)
        {
            satellite.position = SatellitePositions[index];
        }
    }
}