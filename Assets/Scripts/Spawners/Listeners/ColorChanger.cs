using UnityEngine;
using System.Collections.Generic;
using SpawnerSystem.Spawners;

namespace Core.Spawners.Listeners
{
    public class ColorChanger : MonoBehaviour, ISpawnListener<Shape>
    {
        public void OnSpawned(Shape spawned)
        {
            spawned.Color = Random.ColorHSV(
                hueMin: 0f, hueMax: 1f,
                saturationMin: 0.5f, saturationMax: 1f,
                valueMin: 0.25f, valueMax: 1f,
                alphaMin: 1f, alphaMax: 1f);
        }

        public void OnDespawned(Shape despawned) { }
    }
}
