namespace SpawnerSystem.ObjectPooling
{
    public interface IPool<T>
    {
        Poolable<T> Retrieve();
        void RetrieveMany(Poolable<T>[] poolables);
        void RetrieveMany(Poolable<T>[] poolables, int count);

        void Return(Poolable<T> poolable);
    }
}