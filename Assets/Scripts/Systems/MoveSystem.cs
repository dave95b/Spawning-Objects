using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

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


        public override Vector3 GetData(int index)
        {
            return velocities[index];
        }

        protected override void OnAdd(Vector3 data)
        {
            velocities.Add(data);
        }

        protected override void OnRemove(int index)
        {
            velocities.RemoveAtSwapBack(index);
        }

        protected override void OnUpdateData(int index, Vector3 data)
        {
            velocities[index] = data;
        }

        protected override void OnDestroy()
        {
            velocities.Dispose();
            base.OnDestroy();
        }
    }

    [BurstCompile]
    internal struct MoveJob : IJobParallelForTransform
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