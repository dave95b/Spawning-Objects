using NaughtyAttributes;
using SpawnerSystem.ObjectPooling;
using SpawnerSystem.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace SpawnerSystem.Spawners
{
    public abstract class SpawnerPreparer<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private SpawnPoint[] spawnPoints;

        [SerializeField]
        private SelectorProvider selectorProvider;

        protected abstract MultiPoolPreparer<T> PoolPreparer { get; }
        protected virtual List<ISpawnListener<T>> SpawnListeners { get; }

        private Spawner<T> spawner;
        public Spawner<T> Spawner
        {
            get
            {
                if (spawner is null)
                    spawner = CreateSpawner();
                return spawner;
            }
        }


        private Spawner<T> CreateSpawner()
        {
            var pool = PoolPreparer.MultiPool;
            var selector = selectorProvider.Selector;
            var spawnListeners = SpawnListeners ?? new List<ISpawnListener<T>>();

            return new Spawner<T>(pool, spawnPoints, selector, spawnListeners);
        }


        [Conditional("UNITY_EDITOR"), Button]
        protected void InitializeSelector()
        {
            if (selectorProvider == null)
                return;

            GameObject[] spawnPointObjects = spawnPoints.Select(spawnPoint => spawnPoint.gameObject).ToArray();
            selectorProvider.Initialize(spawnPointObjects);
        }

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            InitializeSelector();
        }
    }
}