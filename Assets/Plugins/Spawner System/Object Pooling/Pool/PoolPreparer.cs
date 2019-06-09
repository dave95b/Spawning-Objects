using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace SpawnerSystem.ObjectPooling
{
    public abstract class PoolPreparer<T> : MonoBehaviour
    {
        [SerializeField, MinValue(1)]
        private int size = 10, expandAmount = 5, instantiatedPerFrame = 10;

        protected abstract Poolable<T> Prefab { get; }
        protected virtual IPoolableStateResotrer<T> StateRestorer { get; }
        protected virtual IPoolableFactory<T> PoolableFactory { get; }

        private Pool<T> pool;
        public Pool<T> Pool
        {
            get
            {
                if (pool is null)
                    pool = CreatePool();
                return pool;
            }
        }

        private PoolExpander<T> expander;

        private Pool<T> CreatePool()
        {
            var pooledObjects = new List<Poolable<T>>(size);
            GetPrewarmedObjects(pooledObjects);

            var helper = new PoolHelper<T>(pooledObjects);
            var poolableFactory = PoolableFactory ?? new PoolableFactory<T>(Prefab, transform, StateRestorer);
            expander = new PoolExpander<T>(pooledObjects, expandAmount, instantiatedPerFrame, poolableFactory);
            var pool = new Pool<T>(pooledObjects, helper, expander, StateRestorer);

            poolableFactory.Pool = pool;

            foreach (var pooled in pooledObjects)
                pooled.Pool = pool;

            int toInstantiate = size - pooledObjects.Count;
            if (toInstantiate > 0)
                expander.Expand(toInstantiate);

            return pool;
        }

        private void Update()
        {
            expander?.Update();
        }

#if UNITY_EDITOR
        [Button]
        public void CreateObjects()
        {
            for (int i = 0; i < size; i++)
            {
                var created = UnityEditor.PrefabUtility.InstantiatePrefab(Prefab, transform) as Poolable<T>;
                created.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                StateRestorer?.OnReturn(created);
                PoolableFactory?.OnCreated(created);
            }
        }
#endif

        private void GetPrewarmedObjects(List<Poolable<T>> pooledObjects)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var poolable = transform.GetChild(i).GetComponent<Poolable<T>>();
                if (poolable != null)
                    pooledObjects.Add(poolable);
            }
        }
    }
}
