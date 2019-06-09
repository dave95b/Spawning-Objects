using UnityEngine;
using System.Collections.Generic;
using System;

namespace SpawnerSystem.Spawners
{
    public abstract class SpawnListenerRepository<T> : MonoBehaviour where T : Component
    {
        public abstract ISpawnListener<T>[] Listeners { get; }
    }
}