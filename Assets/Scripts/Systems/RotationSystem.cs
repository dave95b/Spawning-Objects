using UnityEngine;
using System.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Systems
{
    public class RotationSystem : GameSystem<float3>
    {
        private NativeList<float3> angularVelocities;

        protected override void Awake()
        {
            base.Awake();
            angularVelocities = new NativeList<float3>(128, Allocator.Persistent);
        }

        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var job = new RotateJob
            {
                AngularVelocities = angularVelocities,
                DeltaTime = Time.deltaTime
            };

            inputHandle = job.Schedule(transforms, inputHandle);
        }

        protected override void OnAdd(float3 data)
        {
            angularVelocities.Add(data);
        }

        protected override void OnRemove(int index)
        {
            angularVelocities.RemoveAtSwapBack(index);
        }

        protected override void OnDestroy()
        {
            angularVelocities.Dispose();
            base.OnDestroy();
        }
    }

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct RotateJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float3> AngularVelocities;

        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            float3 velocity = math.radians(AngularVelocities[index]) * DeltaTime;
            transform.localRotation *= quaternion.EulerXYZ(velocity);
        }
    }
}