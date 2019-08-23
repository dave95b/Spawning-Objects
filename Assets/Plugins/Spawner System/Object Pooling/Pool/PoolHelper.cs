using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SpawnerSystem.ObjectPooling
{
    public class PoolHelper<T>
    {
        private readonly List<Poolable<T>> pooledObjects;

        public PoolHelper(List<Poolable<T>> pooledObjects)
        {
            Assert.IsNotNull(pooledObjects);
            this.pooledObjects = pooledObjects;
        }


        public Poolable<T> Retrieve()
        {
            int index = pooledObjects.Count - 1;
            var poolable = pooledObjects[index];
            pooledObjects.RemoveAt(index);

            poolable.IsUsed = true;

            return poolable;
        }

        public void Return(Poolable<T> poolable)
        {
            Assert.IsTrue(poolable.IsUsed);

            pooledObjects.Add(poolable);
            poolable.IsUsed = false;
        }
    }
}
