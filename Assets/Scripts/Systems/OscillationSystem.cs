using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace Systems
{
    public class OscillationSystem : GameSystem<OscillationData>
    {
        private NativeList<OscillationData> systemData;

        protected override void Awake()
        {
            base.Awake();
            systemData = new NativeList<OscillationData>(128, Allocator.Persistent);
        }


        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var job = new OscillateJob
            {
                Data = systemData,
                Time = Time.time
            };

            inputHandle = job.Schedule(transforms, inputHandle);
        }


        public override OscillationData GetData(int index)
        {
            return systemData[index];
        }

        protected override void OnAdd(OscillationData data)
        {
            systemData.Add(data);
        }

        protected override void OnRemove(int index)
        {
            systemData.RemoveAtSwapBack(index);
        }

        protected override void OnUpdateData(int index, OscillationData data)
        {
            systemData[index] = data;
        }

        protected override void OnDestroy()
        {
            systemData.Dispose();
            base.OnDestroy();
        }
    }


    public struct OscillationData
    {
        public Vector3 Offset;
        public float Frequency, PreviousOscillation;

        public OscillationData(Vector3 offset, float frequency)
        {
            Offset = offset;
            Frequency = frequency;
            PreviousOscillation = 0f;
        }
    }

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct OscillateJob : IJobParallelForTransform
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