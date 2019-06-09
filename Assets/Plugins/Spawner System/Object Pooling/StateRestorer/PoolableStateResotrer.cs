using UnityEngine;
using System.Collections.Generic;

namespace SpawnerSystem.ObjectPooling
{
    public interface IPoolableStateResotrer<T>
    {
        void OnRetrieve(Poolable<T> poolable);
        void OnReturn(Poolable<T> poolable);
    }

    public class DefaultStateRestorer<T> : IPoolableStateResotrer<T>
    {
        public void OnRetrieve(Poolable<T> poolable)
        {
            poolable.gameObject.SetActive(true);
        }

        public void OnReturn(Poolable<T> poolable)
        {
            poolable.gameObject.SetActive(false);
        }
    }
}