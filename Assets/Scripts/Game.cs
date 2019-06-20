using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using Core.Spawners;
using SpawnerSystem.Spawners;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Systems;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private ShapeSpawnerPreparer spawnerPreparer;

        [SerializeField, FloatRangeSlider(0.1f, 2f)]
        private FloatRange shrinkDuration;

        [SerializeField]
        private ScaleSystem scaleSystem;


        private Spawner<Shape> Spawner => spawnerPreparer.Spawner;
        private List<Shape> shapes = new List<Shape>(36);

        private ActionSource<Shape> actionSource;

        [ShowNativeProperty]
        private int ShapeCount => shapes.Count;


        private void Awake()
        {
            actionSource = new ActionSource<Shape>((shape) => () => Spawner.Despawn(shape));    
        }


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
                Remove(shape);
            shapes.Clear();
        }

        public void RemoveRandom()
        {
            if (shapes.Count == 0)
                return;

            int index = Random.Range(0, shapes.Count);
            var shape = shapes[index];
            Remove(shape);
            shapes.RemoveAtSwapback(index);
        }

        private void Remove(Shape shape)
        {
            scaleSystem.Remove(shape.transform);

            float scale = shape.transform.localScale.x;
            scaleSystem.AddData(shape.transform, shrinkDuration.Random, scale, endScale: 0f, actionSource[shape]);
        }
    }
}