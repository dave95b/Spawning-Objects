using UnityEngine;
using System.Collections;
using Core.Spawners;
using SpawnerSystem.Spawners;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        [SerializeField]
        private KeyCode createKey = KeyCode.C;

        private Spawner<Shape> _spawner;

        private void Start()
        {
            _spawner = spawnerPreparer.Spawner;
        }

        private void Update()
        {
            if (Input.GetKeyDown(createKey))
                Create();
        }

        private void Create()
        {
            _spawner.Spawn();
        }
    }
}