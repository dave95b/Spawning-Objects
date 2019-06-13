using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Spawners.Listeners.Satellites
{
    public interface ISatelliteConfigurator
    {
        void Configure(Shape shape, List<Shape> satellites);
        void OnDespawned(Shape shape, List<Shape> satellites);
    }
}