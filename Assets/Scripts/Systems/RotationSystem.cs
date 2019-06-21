using UnityEngine;
using System.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Systems
{
    public class RotationSystem : GameSystem<Vector3>
    {
        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var job = new RotateJob
            {
                AngularVelocities = dataList,
                DeltaTime = Time.deltaTime
            };

            inputHandle = job.Schedule(transforms, inputHandle);
        }
    }

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct RotateJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<Vector3> AngularVelocities;

        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            float3 velocity = math.radians(AngularVelocities[index]) * DeltaTime;
            transform.localRotation *= quaternion.EulerXYZ(velocity);
        }
    }
}