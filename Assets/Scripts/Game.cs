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

        [SerializeField]
        private KeyCode createKey = KeyCode.C, returnKey = KeyCode.R;

        private Spawner<Shape> _spawner;
        private List<Shape> _shapes = new List<Shape>(36);

        private void Start()
        {
            _spawner = spawnerPreparer.Spawner;
        }

        private void Update()
        {
            if (Input.GetKeyDown(createKey))
                Create();
            else if (Input.GetKeyDown(returnKey))
                Return();
        }

        private void Create()
        {
            Shape shape = _spawner.Spawn();
            _shapes.Add(shape);
        }

        private void Return()
        {
            foreach (var shape in _shapes)
                _spawner.Despawn(shape);
            _shapes.Clear();
        }
    }
}