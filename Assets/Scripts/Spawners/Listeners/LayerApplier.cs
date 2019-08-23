namespace Core.Spawners.Listeners
{
    public class LayerApplier : SpawnZoneListener
    {
        protected override void OnShapeSpawned(Shape spawned)
        {
            spawned.gameObject.layer = gameObject.layer;
        }

        protected override void OnShapeDespawned(Shape despawned) { }
    }
}