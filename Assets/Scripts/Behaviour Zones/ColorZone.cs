using UnityEngine;
using Utilities;

namespace Core.BehaviourZones
{
    public class ColorZone : CachedBehaviourZone<Color[]>
    {
        [SerializeField]
        private ColorRandomizer randomizer;

        protected override Color GizmoColor => randomizer.BaseColor;


        protected override Color[] GetCachedData(Shape shape)
        {
            var colors = new Color[shape.Count];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = shape.GetColor(i);

            return colors;
        }

        protected override void OnShapeEnter(Shape shape, Color[] colors)
        {
            shape.SetColor(randomizer.RandomColor);
        }

        protected override void OnShapeExit(Shape shape, Color[] data)
        {
            for (int i = 0; i < data.Length; i++)
                shape.SetColor(data[i], i);
        }

        protected override void Reset()
        {
            base.Reset();
            enter = true;
            exit = true;
        }
    }
}