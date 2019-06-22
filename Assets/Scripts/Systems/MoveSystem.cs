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
        private NativeList<Vector3> velocities;

        protected override void Awake()
        {
            base.Awake();
            velocities = new NativeList<Vector3>(128, Allocator.Persistent);
        }

        public override void OnUpdate(ref JobHandle inputHandle)
        {
            var job = new MoveJob
            {
                Velocities = velocities,
                DeltaTime = Time.deltaTime
            };

            inputHandle = job.Schedule(transforms, inputHandle); 
        }

        protected override void OnAdd(Vector3 data)
        {
            velocities.Add(data);
        }

        protected override void OnRemove(int index)
        {
            velocities.RemoveAtSwapBack(index);
        }

        protected override void OnDestroy()
        {
            velocities.Dispose();
            base.OnDestroy();
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