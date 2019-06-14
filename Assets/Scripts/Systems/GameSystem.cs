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
        public abstract JobHandle OnUpdate(JobHandle inputHandle);
        public abstract void OnLateUpdate();
    }

    public abstract class GameSystem<T> : GameSystem where T : struct
    {
        protected TransformAccessArray transforms;
        protected NativeList<T> dataList;

        protected Dictionary<Transform, int> transformPositions = new Dictionary<Transform, int>(new TransformComparer());

        protected List<Pair> toAdd = new List<Pair>(32);
        protected List<Transform> toRemove = new List<Transform>(32);


        protected virtual void Awake()
        {
            transforms = new TransformAccessArray(128);
            dataList = new NativeList<T>(128, Allocator.Persistent);
        }

        public override void OnLateUpdate()
        {
            AddScheduled();
            RemoveScheduled();
        }


        public void AddData(Transform transform, T data)
        {
            if (transformPositions.ContainsKey(transform))
                return;

            var pair = new Pair(transform, data);
            toAdd.Add(pair);
        }

        public virtual void Remove(Transform transform)
        {
            if (!transformPositions.ContainsKey(transform))
                return;

            toRemove.Add(transform);
        }

        protected virtual void AddScheduled()
        {
            Assert.AreEqual(transforms.length, transformPositions.Count);

            foreach (var pair in toAdd)
            {
                int index = dataList.Length;
                transformPositions[pair.Transform] = index;

                transforms.Add(pair.Transform);
                dataList.Add(pair.Data);
            }

            toAdd.Clear();
            Assert.AreEqual(transforms.length, transformPositions.Count);
        }

        protected virtual void RemoveScheduled()
        {
            Assert.AreEqual(transforms.length, transformPositions.Count);

            foreach (var transform in toRemove)
            {
                int index = transformPositions[transform];

                if (transforms.length > 0)
                {
                    Transform last = transforms[transforms.length - 1];
                    transformPositions[last] = index;
                }

                transforms.RemoveAtSwapBack(index);
                dataList.RemoveAtSwapBack(index);

                transformPositions.Remove(transform);
            }

            toRemove.Clear();
            Assert.AreEqual(transforms.length, transformPositions.Count);
        }


        protected virtual void OnDestroy()
        {
            transforms.Dispose();
            dataList.Dispose();
        }

        protected struct Pair : IEquatable<Pair>
        {
            public Transform Transform;
            public T Data;

            public Pair(Transform transform, T data)
            {
                Transform = transform;
                Data = data;
            }

            public bool Equals(Pair other)
            {
                return Transform == other.Transform;
            }

            public override bool Equals(object obj)
            {
                if (obj is Pair other)
                    return Equals(other);

                return false;
            }

            public override int GetHashCode()
            {
                return Transform.GetHashCode();
            }
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