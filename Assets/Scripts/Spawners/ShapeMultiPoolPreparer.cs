using SpawnerSystem.ObjectPooling;
using System.Linq;
using UnityEngine;

namespace Core.Spawners
{
    public class ShapeMultiPoolPreparer : MultiPoolPreparer<Shape>
    {
        [SerializeField]
        private ShapePoolPreparer[] poolPreparers;
        protected override PoolPreparer<Shape>[] PoolPreparers => poolPreparers;

        [SerializeField]
        private ShapeMultiPoolPreparer[] multiPoolPreparers;
        protected override MultiPoolPreparer<Shape>[] MultiPoolPreparers => multiPoolPreparers;

        protected override void FindPoolPreparers()
        {
            poolPreparers = GetComponentsInChildren<ShapePoolPreparer>().Where(PreparersPredicate).ToArray();
            multiPoolPreparers = GetComponentsInChildren<ShapeMultiPoolPreparer>().Where(PreparersPredicate).ToArray();

            InitializeSelector();
        }
    }
}
