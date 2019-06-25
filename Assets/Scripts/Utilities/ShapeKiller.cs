using UnityEngine;
using System.Collections;
using Systems;
using Core;
using SpawnerSystem.Spawners;
using System;
using System.Collections.Generic;

namespace Utilities
{
    public class ShapeKiller : MonoBehaviour
    {
        [SerializeField, FloatRangeSlider(0.1f, 2f)]
        private FloatRange duration;

        [SerializeField]
        private ScaleSystem system;

        private ActionSource<Shape> actionSource;

        private void Awake()
        {
            actionSource = new ActionSource<Shape>((shape) => () =>
            {
                system.Remove(shape.transform);
                shape.Spawner.Despawn(shape);
            });
        }

        public void Kill(Shape shape)
        {
            float scale = shape.transform.localScale.x;
            var data = new ScaleSystemData(duration.Random, scale, endScale: 0f, actionSource[shape]);

            system.UpdateData(shape.transform, data);
        }
    }
}