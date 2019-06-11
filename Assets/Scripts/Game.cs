﻿using UnityEngine;
using System.Collections;
using Core.Spawners;
using SpawnerSystem.Spawners;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        private Spawner<Shape> Spawner => spawnerPreparer.Spawner;
        private List<Shape> _shapes = new List<Shape>(36);


        public void Create()
        {
            Shape shape = Spawner.Spawn();
            _shapes.Add(shape);
        }

        public void Create(int count)
        {
            Assert.IsTrue(count >= 0);
            for (int i = 0; i < count; i++)
                Create();
        }

        public void RemoveAll()
        {
            foreach (var shape in _shapes)
                Spawner.Despawn(shape);
            _shapes.Clear();
        }

        public void RemoveRandom()
        {
            if (_shapes.Count == 0)
                return;

            int index = Random.Range(0, _shapes.Count);
            var shape = _shapes[index];
            Spawner.Despawn(shape);
            _shapes.RemoveAtSwapback(index);
        }
    }
}