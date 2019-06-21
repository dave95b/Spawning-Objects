﻿using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using Core.Spawners;
using SpawnerSystem.Spawners;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Systems;
using Utilities;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        [SerializeField]
        private ShapeKiller killer;


        private Spawner<Shape> Spawner => spawnerPreparer.Spawner;
        private List<Shape> shapes = new List<Shape>(36);

        [ShowNativeProperty]
        private int ShapeCount => shapes.Count;


        public void Create()
        {
            Shape shape = Spawner.Spawn();
            shapes.Add(shape);
        }

        public void Create(int count)
        {
            Assert.IsTrue(count >= 0);
            for (int i = 0; i < count; i++)
                Create();
        }

        public void RemoveAll()
        {
            foreach (var shape in shapes)
                killer.Kill(shape);
            shapes.Clear();
        }

        public void RemoveRandom()
        {
            if (shapes.Count == 0)
                return;

            int index = Random.Range(0, shapes.Count);
            var shape = shapes[index];
            killer.Kill(shape);
            shapes.RemoveAtSwapback(index);
        }
    }
}