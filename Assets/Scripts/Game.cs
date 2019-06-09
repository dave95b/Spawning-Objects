using UnityEngine;
using System.Collections;
using Core.Spawners;
using SpawnerSystem.Spawners;
using System.Collections.Generic;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        private Spawner<Shape> _spawner;
        private List<Shape> _shapes = new List<Shape>(36);

        private void Start()
        {
            _spawner = spawnerPreparer.Spawner;
        }

        
        public void Create()
        {
            Shape shape = _spawner.Spawn();
            _shapes.Add(shape);
        }

        public void RemoveAll()
        {
            foreach (var shape in _shapes)
                _spawner.Despawn(shape);
            _shapes.Clear();
        }

        public void RemoveRandom()
        {
            if (_shapes.Count == 0)
                return;

            int index = Random.Range(0, _shapes.Count);
            var shape = _shapes[index];
            _spawner.Despawn(shape);
            _shapes.RemoveAtSwapback(index);
        }
    }
}