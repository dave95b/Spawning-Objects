using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace SpawnerSystem.ObjectPooling
{
    public abstract class Poolable : MonoBehaviour
    {
    }

    public abstract class Poolable<T> : Poolable
    {
        [SerializeField]
        public T Target;

        public Pool<T> Pool;
        [ReadOnly]
        public bool IsUsed;


        private void Reset()
        {
            Target = GetComponent<T>();
        }
    }
}