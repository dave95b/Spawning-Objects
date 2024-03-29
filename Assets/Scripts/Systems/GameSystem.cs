﻿using NaughtyAttributes;
using System.Collections.Generic;
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

        public bool TryGetData(Transform transform, out T data)
        {
            data = default;
            if (!transformPositions.TryGetValue(transform, out int index))
                return false;

            data = GetData(index);
            return true;
        }

        public void AddData(Transform transform, T data)
        {
            Assert.AreEqual(transforms.length, transformPositions.Count);

            if (!transform.gameObject.activeInHierarchy)
                return;
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
            Assert.AreEqual(transforms.length, transformPositions.Count);

            if (!transform.gameObject.activeInHierarchy)
                return;

            if (transformPositions.TryGetValue(transform, out int index))
                OnUpdateData(index, data);
            else
                AddData(transform, data);

            Assert.AreEqual(transforms.length, transformPositions.Count);
        }

        public bool Contains(Transform transform)
        {
            return transformPositions.ContainsKey(transform);
        }

        public abstract T GetData(int index);
        protected abstract void OnAdd(T data);
        protected abstract void OnRemove(int index);
        protected abstract void OnUpdateData(int index, T data);

        protected virtual void OnDestroy()
        {
            transforms.Dispose();
        }
    }

    internal class TransformComparer : IEqualityComparer<Transform>
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