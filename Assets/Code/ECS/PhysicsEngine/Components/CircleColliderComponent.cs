using Unity.Entities;

namespace RedsAndBlues.ECS.PhysicsEngine.Components
{
    public struct CircleColliderComponent : IComponentData
    {
        public float Radius;
        public CollisionLayer Group;
    }
}