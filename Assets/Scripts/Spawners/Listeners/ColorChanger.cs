using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Core.Spawners.Zones;

namespace Core.Spawners.Listeners
{
    public class ColorChanger : SpawnZoneListener
    {
        [SerializeField]
        private Color baseColor;

        [SerializeField, FloatRangeSlider(0f, 1f)]
        private FloatRange hueRange, saturationRange, valueRange;

        [SerializeField]
        private bool uniformColors;

        private Color RandomColor
        {
            get
            {
                Color.RGBToHSV(baseColor, out float hue, out float saturation, out float value);

                float randomHue = hueRange.RandomRange;
                float randomSaturation = saturationRange.RandomRange;
                float randomValue = valueRange.RandomRange;

                return Random.ColorHSV(
                                hueMin: hue - randomHue,
                                hueMax: hue + randomHue,
                                saturationMin: saturation - randomSaturation,
                                saturationMax: saturation + randomSaturation,
                                valueMin: value - randomValue,
                                valueMax: value + randomValue,
                                alphaMin: 1f, alphaMax: 1f);
            }
        }

        protected override void DoOnSpawned(Shape spawned)
        {
            if (uniformColors)
                spawned.SetColor(RandomColor);
            else
            {
                for (int i = 0; i < spawned.Count; i++)
                    spawned.SetColor(RandomColor, i);
            }
        }

        protected override void DoOnDespawned(Shape despawned) { }
    }
}
