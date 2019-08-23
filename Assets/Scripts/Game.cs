using Core.Spawners;
using NaughtyAttributes;
using SpawnerSystem.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private int createPerFrame = 15;

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
            StartCoroutine(CreateCoroutine(count));
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

        private IEnumerator CreateCoroutine(int count)
        {
            while (count > 0)
            {
                int toCreate = Mathf.Min(count, createPerFrame);

                for (int i = 0; i < toCreate; i++)
                    Create();

                count -= toCreate;
                yield return null;
            }
        }
    }
}