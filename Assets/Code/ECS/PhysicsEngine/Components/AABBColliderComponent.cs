using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.ECS.PhysicsEngine.Components
{
    public struct AABBColliderComponent : IComponentData
    {
        public float2 Size;
    }
}