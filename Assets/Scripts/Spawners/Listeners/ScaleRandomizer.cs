using UnityEngine;
using Systems;

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

        protected override void DoOnSpawned(Shape spawned)
        {
            float _scale = scale.Random;
            spawned.Scale = _scale;
            spawned.transform.localScale = Vector3.zero;
            system.AddData(spawned.transform, duration.Random, startScale: 0f, _scale, actionSource[spawned]);
        }

        protected override void DoOnDespawned(Shape despawned)
        {
            system.Remove(despawned.transform);
        }
    }
}
