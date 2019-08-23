using System.Collections.Generic;

namespace Core.Spawners.Listeners.Satellites
{
    public interface ISatelliteConfigurator
    {
        void Configure(Shape shape, List<Shape> satellites);
    }
}