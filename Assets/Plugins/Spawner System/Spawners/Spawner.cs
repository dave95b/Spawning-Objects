using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using SpawnerSystem.Shared;
using SpawnerSystem.ObjectPooling;
using System;

namespace SpawnerSystem.Spawners
{
    public class Spawner<T> : ISpawner<T> where T : Component
    {
        private readonly IPool<T> pool;
        private readonly ISpawnPoint[] spawnPoints;
        private readonly ISelector spawnPointSelector;
        private readonly List<ISpawnListener<T>> spawnListeners;

        private readonly Dictionary<T, Poolable<T>> spawnedPoolables;
        private Poolable<T>[] poolableArray;

        
        public Spawner(IPool<T> pool, ISpawnPoint[] spawnPoints, ISelector spawnPointSelector) : this(pool, spawnPoints, spawnPointSelector, new List<ISpawnListener<T>>())
        {
            
        }

        public Spawner(IPool<T> pool, ISpawnPoint[] spawnPoints, ISelector spawnPointSelector, List<ISpawnListener<T>> spawnListeners)
        {
            Assert.IsNotNull(pool);
            Assert.IsNotNull(spawnPoints);
            Assert.IsNotNull(spawnPointSelector);

            this.pool = pool;
            this.spawnPoints = spawnPoints;
            this.spawnPointSelector = spawnPointSelector;
            this.spawnListeners = spawnListeners;

            spawnedPoolables = new Dictionary<T, Poolable<T>>();
            poolableArray = new Poolable<T>[16];
        }


        public T Spawn()
        {
            ISpawnPoint spawnPoint = SelectSpawnPoint();
            return Spawn(spawnPoint);
        }

        public T Spawn(ISpawnPoint spawnPoint)
        {
            Assert.IsNotNull(spawnPoint);

            Poolable<T> poolable = pool.Retrieve();
            Initialize(poolable, spawnPoint);

            return poolable.Target;
        }

        public void SpawnMany(T[] spawnedArray)
        {
            SpawnMany(spawnedArray, spawnedArray.Length);
        }

        public void SpawnMany(T[] spawnedArray, int count)
        {
            Assert.IsNotNull(spawnedArray);
            Assert.IsTrue(count <= spawnedArray.Length);

            CheckPoolableArraySize(count);
            pool.RetrieveMany(poolableArray, count);

            for (int i = 0; i < count; i++)
            {
                ISpawnPoint spawnPoint = SelectSpawnPoint();
                Initialize(spawnedArray, i, spawnPoint);
            }
        }

        public void SpawnMany(T[] spawnedArray, ISpawnPoint spawnPoint)
        {
            SpawnMany(spawnedArray, spawnedArray.Length, spawnPoint);
        }

        public void SpawnMany(T[] spawnedArray, int count, ISpawnPoint spawnPoint)
        {
            Assert.IsNotNull(spawnedArray);
            Assert.IsTrue(count <= spawnedArray.Length);

            CheckPoolableArraySize(count);
            pool.RetrieveMany(poolableArray, count);

            for (int i = 0; i < count; i++)
                Initialize(spawnedArray, i, spawnPoint);
        }

        public void Despawn(T spawned)
        {
            Assert.IsNotNull(spawned);
            Assert.IsTrue(spawnedPoolables.ContainsKey(spawned));

            Poolable<T> poolable = spawnedPoolables[spawned];
            if (!poolable.IsUsed)
                return;

            foreach (var listener in spawnListeners)
                listener.OnDespawned(spawned);

            pool.Return(poolable);
        }

        public void AddListener(ISpawnListener<T> listener)
        {
            spawnListeners.Add(listener);
        }

        public void RemoveListener(ISpawnListener<T> listener)
        {
            spawnListeners.Remove(listener);
        }

        private ISpawnPoint SelectSpawnPoint()
        {
            int spawnPointIndex = spawnPointSelector.SelectIndex();
            Assert.IsTrue(spawnPointIndex < spawnPoints.Length);

            return spawnPoints[spawnPointIndex];
        }

        private void Initialize(T[] spawnedArray, int index, ISpawnPoint spawnPoint)
        {
            Poolable<T> poolable = poolableArray[index];
            Initialize(poolable, spawnPoint);
            spawnedArray[index] = poolable.Target;
        }

        private void Initialize(Poolable<T> poolable, ISpawnPoint spawnPoint)
        {
            T spawned = poolable.Target;
            Assert.IsNotNull(spawned);

            spawnPoint.Apply(spawned);
            spawnedPoolables[spawned] = poolable;

            foreach (var listener in spawnListeners)
                listener.OnSpawned(spawned);
        }

        private void CheckPoolableArraySize(int count)
        {
            if (poolableArray.Length < count)
                poolableArray = new Poolable<T>[count * 2];
        }
    }
}