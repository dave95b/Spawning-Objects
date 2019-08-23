using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Jobs;

namespace Systems
{
    public class ScaleSystem : GameSystem<ScaleSystemData>
    {
        private NativeList<ScaleRanges> scaleRanges;
        private NativeList<float> times, durations, scales;
        private NativeList<int> finishedIndices;

        private List<Action> onFinishedActions = new List<Action>(32);
        private List<Action> toInvoke = new List<Action>(32);


        protected override void Awake()
        {
            scaleRanges = new NativeList<ScaleRanges>(128, Allocator.Persistent);
            times = new NativeList<float>(128, Allocator.Persistent);
            durations = new NativeList<float>(128, Allocator.Persistent);
            scales = new NativeList<float>(128, Allocator.Persistent);
            finishedIndices = new NativeList<int>(128, Allocator.Persistent);
            base.Awake();
        }


        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var scaleJob = new CalculateScaleJob
            {
                ScaleRanges = scaleRanges,
                Durations = durations,
                Times = times,
                Scales = scales,
                DeltaTime = Time.deltaTime
            };

            var applyJob = new ApplyScaleJob
            {
                Scales = scales
            };

            inputHandle = scaleJob.Schedule(scaleRanges.Length, 512, inputHandle);
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


        public override ScaleSystemData GetData(int index)
        {
            var scaleRange = scaleRanges[index];
            return new ScaleSystemData(durations[index], scaleRange.StartScale, scaleRange.EndScale, onFinishedActions[index]);
        }

        protected override void OnAdd(ScaleSystemData data)
        {
            var ranges = new ScaleRanges(data.StartScale, data.EndScale);
            scaleRanges.Add(ranges);

            times.Add(0f);
            scales.Add(transform.localScale.x);
            durations.Add(data.Duration);
            onFinishedActions.Add(data.OnFinished);
        }

        protected override void OnRemove(int index)
        {
            Assert.AreEqual(times.Length, transformPositions.Count);

            scaleRanges.RemoveAtSwapBack(index);
            times.RemoveAtSwapBack(index);
            durations.RemoveAtSwapBack(index);
            scales.RemoveAtSwapBack(index);
            onFinishedActions.RemoveAtSwapback(index);
        }

        protected override void OnUpdateData(int index, ScaleSystemData data)
        {
            var ranges = new ScaleRanges(data.StartScale, data.EndScale);
            scaleRanges[index] = ranges;

            times[index] = 0f;
            scales[index] = transform.localScale.x;
            durations[index] = data.Duration;
            onFinishedActions[index] = data.OnFinished;
        }


        protected override void OnDestroy()
        {
            scaleRanges.Dispose();
            times.Dispose();
            durations.Dispose();
            scales.Dispose();
            finishedIndices.Dispose();
            base.OnDestroy();
        }
    }

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct CalculateScaleJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<ScaleRanges> ScaleRanges;

        [ReadOnly]
        public NativeArray<float> Durations;

        [ReadOnly]
        public float DeltaTime;

        public NativeArray<float> Times;

        [WriteOnly]
        public NativeArray<float> Scales;


        public void Execute(int index)
        {
            var ranges = ScaleRanges[index];
            float duration = Durations[index];
            float time = math.min(Times[index] + DeltaTime, duration);
            Times[index] = time;

            float t = math.unlerp(0f, duration, time);
            t = (3f - 2f * t) * t * t;
            float scale = math.lerp(ranges.StartScale, ranges.EndScale, t);

            Scales[index] = scale;
        }
    }

    [BurstCompile]
    internal struct ApplyScaleJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float> Scales;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localScale = Vector3.one * Scales[index];
        }
    }

    [BurstCompile]
    internal struct FindFinishedJob : IJob
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
        public readonly float Duration, StartScale, EndScale;
        public readonly Action OnFinished;

        public ScaleSystemData(float duration, float startScale, float endScale, Action onFinished = null)
        {
            Duration = duration;
            StartScale = startScale;
            EndScale = endScale;
            OnFinished = onFinished;
        }
    }

    public readonly struct ScaleRanges
    {
        public readonly float StartScale, EndScale;

        public ScaleRanges(float startScale, float endScale)
        {
            StartScale = startScale;
            EndScale = endScale;
        }
    }
}