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
        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var job = new MoveJob
            {
                Velocities = dataList,
                DeltaTime = Time.deltaTime
            };

            inputHandle = job.Schedule(transforms, inputHandle); 
        }
    }

    [BurstCompile]
    struct MoveJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<Vector3> Velocities;

        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localPosition += Velocities[index] * DeltaTime;
        }
    }
}