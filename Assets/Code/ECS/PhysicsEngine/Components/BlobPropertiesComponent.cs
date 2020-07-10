using Unity.Entities;

namespace RedsAndBlues.ECS.PhysicsEngine.Components
{
    public struct BlobPropertiesComponent : IComponentData
    {
        public float DestroyRadius;
    }
}