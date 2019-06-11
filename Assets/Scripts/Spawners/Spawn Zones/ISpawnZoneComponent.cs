using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Spawners.Zones
{
    public interface ISpawnZoneComponent
    {
        void OnSpawnedInZone(Shape shape);
        void OnDesawnedInZone(Shape shape);
    }
}