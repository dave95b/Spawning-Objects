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
        public abstract void Remove(Transform transform);
    }

    public abstract class GameSystem<T> : GameSystem where T : struct
    {
        protected TransformAccessArray transforms;

        protected Dictionary<Transform, int> transformPositions = new Dictionary<Transform, int>(new TransformComparer());

        [ShowNativeProperty]
        private int DataCount => transforms.isCreated ? transforms.length : -1;


        protected virtual void Awake()
        {
            transforms = new TransformAccessArray(128, 12);
        }

        public void AddData(Transform transform, T data)
        {
            if (!transform.gameObject.activeInHierarchy)
                return;

            Assert.AreEqual(transforms.length, transformPositions.Count);

            if (transformPositions.ContainsKey(transform))
                return;

            int index = transforms.length;
            transformPositions[transform] = index;
            transforms.Add(transform);

            OnAdd(data);
            Assert.AreEqual(transforms.length, transformPositions.Count);
        }

        public override void Remove(Transform transform)
        {
            Assert.AreEqual(transforms.length, transformPositions.Count);

            if (!transformPositions.TryGetValue(transform, out int index))
                return;

            OnRemove(index);

            if (transforms.length > 0)
            {
                Transform last = transforms[transforms.length - 1];
                transformPositions[last] = index;
            }

            transforms.RemoveAtSwapBack(index);
            transformPositions.Remove(transform);

            Assert.AreEqual(transforms.length, transformPositions.Count);
        }

        public void UpdateData(Transform transform, T data)
        {
            if (transformPositions.TryGetValue(transform, out int index))
                OnUpdateData(index, data);
            else
                AddData(transform, data);
        }

        protected abstract void OnAdd(T data);
        protected abstract void OnRemove(int index);
        protected abstract void OnUpdateData(int index, T data);

        protected virtual void OnDestroy()
        {
            transforms.Dispose();
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