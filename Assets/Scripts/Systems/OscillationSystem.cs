using UnityEngine;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Systems
{
    public class OscillationSystem : GameSystem<OscillationData>
    {
        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var job = new OscillateJob
            {
                Data = dataList,
                Time = Time.time
            };

            inputHandle = job.Schedule(transforms, inputHandle);
        }
    }


    public struct OscillationData
    {
        public Vector3 Offset;
        public float Frequency, PreviousOscillation;

        public OscillationData(Vector3 offset, float frequency) : this()
        {
            Offset = offset;
            Frequency = frequency;
            PreviousOscillation = 0f;
        }
    }

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct OscillateJob : IJobParallelForTransform
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<OscillationData> Data;

        [ReadOnly]
        public float Time;

        public void Execute(int index, TransformAccess transform)
        {
            var data = Data[index];
            float oscillation = math.sin(2f * math.PI * data.Frequency * Time);
            transform.localPosition += (oscillation - data.PreviousOscillation) * data.Offset;

            data.PreviousOscillation = oscillation;
            Data[index] = data;
        }
    }
}