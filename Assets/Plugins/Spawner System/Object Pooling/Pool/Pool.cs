using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SpawnerSystem.ObjectPooling
{
    public class Pool<T> : IPool<T>
    {
        private readonly List<Poolable<T>> pooledObjects;
        private readonly PoolHelper<T> helper;
        private readonly PoolExpander<T> expander;
        private readonly IPoolableStateResotrer<T> stateResotrer;
        

        public Pool(List<Poolable<T>> pooledObjects, PoolHelper<T> helper, PoolExpander<T> expander, IPoolableStateResotrer<T> stateResotrer)
        {
            Assert.IsNotNull(pooledObjects);
            Assert.IsNotNull(helper);
            Assert.IsNotNull(expander);

            this.pooledObjects = pooledObjects;
            this.helper = helper;
            this.expander = expander;
            this.stateResotrer = stateResotrer;
        }


        public Poolable<T> Retrieve()
        {
            if (pooledObjects.Count == 0)
                expander.Expand();

            var poolable = helper.Retrieve();
            Assert.IsNotNull(poolable);
            stateResotrer?.OnRetrieve(poolable);

            return poolable;
        }

        public void RetrieveMany(Poolable<T>[] poolables)
        {
            RetrieveMany(poolables, poolables.Length);
        }

        public void RetrieveMany(Poolable<T>[] poolables, int count)
        {
            Assert.IsNotNull(poolables);
            Assert.IsTrue(count > 0);
            Assert.IsTrue(count <= poolables.Length);

            for (int i = 0; i < count; i++)
            {
                var poolable = Retrieve();
                poolables[i] = poolable;
            }
        }

        public void Return(Poolable<T> poolable)
        {
            Assert.IsNotNull(poolable);
            stateResotrer?.OnReturn(poolable);
            helper.Return(poolable);
        }
    }
}
