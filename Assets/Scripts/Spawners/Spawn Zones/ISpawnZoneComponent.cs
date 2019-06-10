using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Spawners.Zones
{
    public interface ISpawnZoneComponent
    {
        void Apply(Shape shape);
    }
}