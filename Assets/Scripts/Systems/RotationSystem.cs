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
        private NativeList<Quaternion> angularVelocities;
        private float deltaTime;

        protected override void Awake()
        {
            base.Awake();
            angularVelocities = new NativeList<Quaternion>(128, Allocator.Persistent);
        }

        public override void OnUpdate(ref JobHandle inputHandle)
        {
            deltaTime = Time.fixedDeltaTime;

            var job = new RotateJob
            {
                AngularVelocities = angularVelocities
            };

            inputHandle = job.Schedule(transforms, inputHandle);
        }

        protected override void OnAdd(Vector3 data)
        {
            angularVelocities.Add(Quaternion.Euler(data * deltaTime));
        }

        protected override void OnRemove(int index)
        {
            angularVelocities.RemoveAtSwapBack(index);
        }

        protected override void OnUpdateData(int index, Vector3 data)
        {
            angularVelocities[index] = Quaternion.Euler(data * deltaTime);
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
        public NativeArray<Quaternion> AngularVelocities;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localRotation *= AngularVelocities[index];
        }
    }
}