using UnityEngine;
using System.Collections.Generic;
using Systems;

namespace Core.Spawners.Listeners
{
    public class SatelliteCleaner : SpawnZoneListener
    {
        [SerializeField]
        private GameSystem[] systems;


        protected override void OnShapeDespawned(Shape despawned)
        {
            foreach (var system in systems)
                system.Remove(despawned.transform);
        }

        protected override void OnShapeSpawned(Shape spawned) { }
    }
}