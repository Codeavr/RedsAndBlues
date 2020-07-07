using Unity.Entities;

namespace RedsAndBlues.Code.PhysicsEngine.Components
{
    public struct CircleColliderComponent : IComponentData
    {
        public float Radius;
        public CollisionLayer Group;
    }
}