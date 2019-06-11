using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Systems
{
    public abstract class GameSystem : MonoBehaviour
    {
        public abstract JobHandle OnUpdate(JobHandle inputHandle);
    }

    public abstract class GameSystem<T> : GameSystem where T : struct
    {
        protected TransformAccessArray transforms;
        protected NativeList<T> dataList;

        protected JobHandle handle;
        protected Dictionary<Transform, int> transformPositions = new Dictionary<Transform, int>();


        protected virtual void Awake()
        {
            transforms = new TransformAccessArray(128);
            dataList = new NativeList<T>(128, Allocator.Persistent);
        }


        public void AddData(Transform transform, T data)
        {
            int index = dataList.Length;
            transformPositions[transform] = index;

            transforms.Add(transform);
            dataList.Add(data);
        }

        public void Remove(Transform transform)
        {
            int index = transformPositions[transform];
            Transform last = transforms[transforms.length - 1];

            transforms.RemoveAtSwapBack(index);
            dataList.RemoveAtSwapBack(index);

            transformPositions[last] = index;
        }


        protected virtual void OnDestroy()
        {
            transforms.Dispose();
            dataList.Dispose();
        }
    }
}