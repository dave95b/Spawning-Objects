using UnityEngine;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;

namespace Systems
{
    public class MoveSystem : GameSystem<Vector3>
    {
        public override JobHandle OnUpdate(JobHandle inputHandle)
        {
            var job = new MoveJob
            {
                Velocities = dataList,
                DeltaTime = Time.deltaTime
            };

            job.Schedule(transforms, inputHandle);

            return inputHandle;
        }
    }

    [BurstCompile]
    struct MoveJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeList<Vector3> Velocities;

        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localPosition += Velocities[index] * DeltaTime;
        }
    }
}