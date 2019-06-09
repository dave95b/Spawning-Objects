using UnityEngine;
using UnityEngine.Assertions;

namespace SpawnerSystem.ObjectPooling
{
    public interface IPoolableFactory<T>
    {
        Pool<T> Pool { get; set; }
        Poolable<T> Create();
        void OnCreated(Poolable<T> created);
    }

    public class PoolableFactory<T> : IPoolableFactory<T>
    {
        public Pool<T> Pool { get; set; }
        private readonly Poolable<T> prefab;
        private readonly Transform parent;
        private readonly IPoolableStateResotrer<T> stateResotrer;

        public PoolableFactory(Poolable<T> prefab, Transform parent, IPoolableStateResotrer<T> stateResotrer)
        {
            Assert.IsNotNull(prefab);
            Assert.IsNotNull(parent);

            this.prefab = prefab;
            this.parent = parent;
            this.stateResotrer = stateResotrer;
        }

        public Poolable<T> Create()
        {
            var created = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            created.Pool = Pool;
            stateResotrer?.OnReturn(created);
            OnCreated(created);
            return created;
        }

        public virtual void OnCreated(Poolable<T> created)
        {

        }
    }
}