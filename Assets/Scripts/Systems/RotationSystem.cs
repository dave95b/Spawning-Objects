using UnityEngine;
using System.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;

namespace Systems
{
    public class RotationSystem : GameSystem<Vector3>
    {
        public override JobHandle OnUpdate(JobHandle inputHandle)
        {
            var job = new RotateJob
            {
                AngularVelocities = dataList,
                DeltaTime = Time.deltaTime
            };

            return job.Schedule(transforms, inputHandle);
        }

        protected override void OnAddScheduled() { }
        protected override void OnRemoveScheduled(Transform transform) { }
    }

    [BurstCompile]
    struct RotateJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<Vector3> AngularVelocities;

        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localRotation *= Quaternion.Euler(AngularVelocities[index] * DeltaTime);
        }
    }
}