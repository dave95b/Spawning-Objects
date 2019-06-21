using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Jobs;

namespace Systems
{
    public abstract class GameSystem : MonoBehaviour
    {
        public abstract void OnUpdate(ref JobHandle inputHandle);
        public virtual void OnLateUpdate() { }
    }

    public abstract class GameSystem<T> : GameSystem where T : struct
    {
        protected TransformAccessArray transforms;
        protected NativeList<T> dataList;

        protected Dictionary<Transform, int> transformPositions = new Dictionary<Transform, int>(new TransformComparer());

        [ShowNativeProperty]
        private int DataCount => dataList.IsCreated ? dataList.Length : -1;


        protected virtual void Awake()
        {
            transforms = new TransformAccessArray(128, 12);
            dataList = new NativeList<T>(128, Allocator.Persistent);
        }

        public bool AddData(Transform transform, T data)
        {
            if (!transform.gameObject.activeInHierarchy)
                return false;

            Assert.AreEqual(transforms.length, transformPositions.Count);

            if (transformPositions.ContainsKey(transform))
                return false;

            int index = dataList.Length;
            transformPositions[transform] = index;

            transforms.Add(transform);
            dataList.Add(data);

            Assert.AreEqual(transforms.length, transformPositions.Count);

            return true;
        }

        public virtual void Remove(Transform transform)
        {
            Assert.AreEqual(transforms.length, transformPositions.Count);

            if (!transformPositions.ContainsKey(transform))
                return;

            int index = transformPositions[transform];
            OnRemove(index);

            if (transforms.length > 0)
            {
                Transform last = transforms[transforms.length - 1];
                transformPositions[last] = index;
            }

            transforms.RemoveAtSwapBack(index);
            dataList.RemoveAtSwapBack(index);

            transformPositions.Remove(transform);

            Assert.AreEqual(transforms.length, transformPositions.Count);
        }

        protected virtual void OnRemove(int index) { }

        protected virtual void OnDestroy()
        {
            transforms.Dispose();
            dataList.Dispose();
        }
    }

    class TransformComparer : IEqualityComparer<Transform>
    {
        public bool Equals(Transform x, Transform y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(Transform obj)
        {
            return obj.GetHashCode();
        }
    }
}