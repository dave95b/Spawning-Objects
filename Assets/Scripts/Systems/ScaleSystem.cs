using System;
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
    public class ScaleSystem : GameSystem<ScaleSystemData>
    {
        private NativeList<float> times, durations, scales;
        private NativeList<int> finishedIndices;

        private List<Action> onFinishedActions = new List<Action>(32);
        private List<Action> toInvoke = new List<Action>(32);


        protected override void Awake()
        {
            times = new NativeList<float>(128, Allocator.Persistent);
            durations = new NativeList<float>(128, Allocator.Persistent);
            scales = new NativeList<float>(128, Allocator.Persistent);
            finishedIndices = new NativeList<int>(128, Allocator.Persistent);
            base.Awake();
        }


        public void AddData(Transform transform, float duration, float startScale, float endScale, Action onFinished = null)
        {
            var data = new ScaleSystemData(startScale, endScale);
            if (AddData(transform, data))
            {
                times.Add(0f);
                scales.Add(transform.localScale.x);
                durations.Add(duration);
                onFinishedActions.Add(onFinished);
            }
        }

        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var scaleJob = new CalculateScaleJob
            {
                Data = dataList,
                Durations = durations,
                Times = times,
                Scales = scales,
                DeltaTime = Time.deltaTime
            };

            var applyJob = new ApplyScaleJob
            {
                Scales = scales
            };

            inputHandle = scaleJob.Schedule(dataList.Length, 512, inputHandle);
            inputHandle = applyJob.Schedule(transforms, inputHandle);
        }

        public override void OnLateUpdate()
        {
            var findFinishedJob = new FindFinishedJob
            {
                Durations = durations,
                Times = times,
                FinishedIndices = finishedIndices
            };

            findFinishedJob.Schedule().Complete();
            for (int i = finishedIndices.Length - 1; i >= 0; i--)
            {
                int index = finishedIndices[i];
                var action = onFinishedActions[index];
                if (action != null)
                    toInvoke.Add(action);
            }

            foreach (var action in toInvoke)
                action();

            finishedIndices.Clear();
            toInvoke.Clear();
        }

        protected override void OnRemove(int index)
        {
            Assert.AreEqual(times.Length, transformPositions.Count);

            times.RemoveAtSwapBack(index);
            durations.RemoveAtSwapBack(index);
            scales.RemoveAtSwapBack(index);
            onFinishedActions.RemoveAtSwapback(index);
        }


        protected override void OnDestroy()
        {
            times.Dispose();
            durations.Dispose();
            scales.Dispose();
            finishedIndices.Dispose();
            base.OnDestroy();
        }
    }

    [BurstCompile]
    struct CalculateScaleJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<ScaleSystemData> Data;

        [ReadOnly]
        public NativeArray<float> Durations;

        [ReadOnly]
        public float DeltaTime;

        public NativeArray<float> Times;

        [WriteOnly]
        public NativeArray<float> Scales;
        

        public void Execute(int index)
        {
            var data = Data[index];
            float duration = Durations[index];
            float time = math.min(Times[index] + DeltaTime, duration);
            Times[index] = time;

            float t = math.unlerp(0f, duration, time);
            t = (3f - 2f * t) * t * t;
            float scale = math.lerp(data.StartScale, data.EndScale, t);

            Scales[index] = scale;
        }
    }

    [BurstCompile]
    struct ApplyScaleJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float> Scales;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localScale = Vector3.one * Scales[index];
        }
    }

    [BurstCompile]
    struct FindFinishedJob : IJob
    {
        [ReadOnly]
        public NativeArray<float> Times, Durations;

        [WriteOnly]
        public NativeList<int> FinishedIndices;

        public void Execute()
        {
            int length = Times.Length;

            for (int i = 0; i < length; i++)
            {
                if (Times[i] >= Durations[i])
                    FinishedIndices.Add(i);
            }
        }
    }

    public readonly struct ScaleSystemData
    {
        public readonly float StartScale, EndScale;

        public ScaleSystemData(float startScale, float endScale)
        {
            StartScale = startScale;
            EndScale = endScale;
        }
    }
}