using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;
using Core.Spawners.Zones;
using Utilities;

namespace Core.Spawners.Listeners
{
    public class ColorChanger : SpawnZoneListener
    {
        [SerializeField]
        private ColorRandomizer randomizer;

        [SerializeField]
        private bool uniformColors;

        protected override void OnShapeSpawned(Shape spawned)
        {
            if (uniformColors)
                spawned.SetColor(randomizer.RandomColor);
            else
            {
                for (int i = 0; i < spawned.Count; i++)
                    spawned.SetColor(randomizer.RandomColor, i);
            }
        }

        protected override void OnShapeDespawned(Shape despawned) { }
    }
}
