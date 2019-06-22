using UnityEngine;
using Systems;
using Utilities;

namespace Core.Spawners.Listeners
{
    public class ScaleRandomizer : SpawnZoneListener
    {
        [SerializeField, FloatRangeSlider(0.1f, 2f)]
        private FloatRange scale, duration;

        [SerializeField]
        private ScaleSystem system;

        private ActionSource<Shape> actionSource;


        private void Awake()
        {
            actionSource = new ActionSource<Shape>((shape) => () => system.Remove(shape.transform));    
        }

        protected override void OnShapeSpawned(Shape spawned)
        {
            float _scale = scale.Random;
            spawned.Scale = _scale;
            spawned.transform.localScale = Vector3.zero;

            var data = new ScaleSystemData(duration.Random, startScale: 0f, _scale, actionSource[spawned]);
            system.AddData(spawned.transform, data);
        }

        protected override void OnShapeDespawned(Shape despawned)
        {
            system.Remove(despawned.transform);
        }
    }
}
