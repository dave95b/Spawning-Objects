namespace Core.Spawners.Zones
{
    public interface ISpawnZoneComponent
    {
        void OnSpawnedInZone(Shape shape);
        void OnDesawnedInZone(Shape shape);
    }
}