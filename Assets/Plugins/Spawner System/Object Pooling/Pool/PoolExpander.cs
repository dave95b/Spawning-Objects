using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SpawnerSystem.ObjectPooling
{
    public class PoolExpander<T>
    {
        private readonly List<Poolable<T>> pooledObjects;
        private readonly int expandAmount, instantiatedPerFrame;
        private readonly IPoolableFactory<T> poolableFactory;

        private int instantiateAmount = 0;

        public PoolExpander(List<Poolable<T>> pooledObjects, int expandAmount, int instantiatedPerFrame, IPoolableFactory<T> poolableFactory)
        {
            Assert.IsNotNull(pooledObjects);
            Assert.IsTrue(expandAmount > 0);
            Assert.IsTrue(instantiatedPerFrame > 0);
            Assert.IsNotNull(poolableFactory);

            this.pooledObjects = pooledObjects;
            this.expandAmount = expandAmount;
            this.instantiatedPerFrame = instantiatedPerFrame;
            this.poolableFactory = poolableFactory;
        }


        public void Expand()
        {
            Expand(expandAmount);
        }

        public void Expand(int amount)
        {
            Assert.IsTrue(amount > 0);

            if (amount <= instantiatedPerFrame)
                CreatePoolables(amount);
            else
            {
                CreatePoolables(instantiatedPerFrame);
                instantiateAmount = amount - instantiatedPerFrame;
            }
        }

        public void Update()
        {
            if (instantiateAmount == 0)
                return;

            int toInstantiate = Mathf.Min(instantiatedPerFrame, instantiateAmount);
            instantiateAmount -= toInstantiate;
            CreatePoolables(toInstantiate);
        }

        private void CreatePoolables(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var created = poolableFactory.Create();
                pooledObjects.Add(created);
            }
        }
    }
}