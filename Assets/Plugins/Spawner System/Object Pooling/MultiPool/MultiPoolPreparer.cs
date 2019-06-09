using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using NaughtyAttributes;
using System.Diagnostics;
using SpawnerSystem.Shared;

namespace SpawnerSystem.ObjectPooling
{
    public abstract class MultiPoolPreparer<T> : MonoBehaviour
    {
        [SerializeField]
        private SelectorProvider selectorProvider;

        protected abstract PoolPreparer<T>[] PoolPreparers { get; }
        protected abstract MultiPoolPreparer<T>[] MultiPoolPreparers { get; }
        protected virtual IPoolableStateResotrer<T> StateRestorer { get; }

        private MultiPool<T> multiPool;
        public MultiPool<T> MultiPool
        {
            get
            {
                if (multiPool is null)
                    multiPool = CreateMultiPool();
                return multiPool;
            }
        }

        private MultiPool<T> CreateMultiPool()
        {
            var pools = GetPools();
            var selector = selectorProvider.Selector;

            return new MultiPool<T>(pools, selector, StateRestorer);
        }

        private IPool<T>[] GetPools()
        {
            int poolCount = PoolPreparers.Length;
            int multiPoolCount = MultiPoolPreparers.Length;
            var pools = new IPool<T>[poolCount + multiPoolCount];

            int i;
            for (i = 0; i < poolCount; i++)
                pools[i] = PoolPreparers[i].Pool;

            for (int j = 0; j < multiPoolCount; j++)
            {
                pools[i] = MultiPoolPreparers[j].MultiPool;
                i++;
            }

            return pools;
        }

#if UNITY_EDITOR
        [Button]
        protected abstract void FindPoolPreparers();

        [Button]
        protected void InitializeSelector()
        {
            if (selectorProvider == null)
                return;

            IEnumerable<GameObject> preparers = PoolPreparers
                .Select(preparer => preparer.gameObject);
            IEnumerable<GameObject> multiPreparers = MultiPoolPreparers.Select(preparer => preparer.gameObject);
            GameObject[] preparerObjects = preparers.Concat(multiPreparers).ToArray();
            selectorProvider.Initialize(preparerObjects);
        }

        private void OnValidate()
        {
            InitializeSelector();
        }

        [Button]
        public void CreateObjects()
        {
            foreach (var preparer in PoolPreparers)
                preparer.CreateObjects();
            foreach (var preparer in MultiPoolPreparers)
                preparer.CreateObjects();
        }

        protected bool PreparersPredicate(PoolPreparer<T> preparer)
        {
            return preparer != this && preparer.transform.parent == transform;
        }

        protected bool PreparersPredicate(MultiPoolPreparer<T> preparer)
        {
            return preparer != this && preparer.transform.parent == transform;
        }
#endif
    }
}
