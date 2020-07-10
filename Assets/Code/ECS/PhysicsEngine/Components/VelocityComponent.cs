using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.ECS.PhysicsEngine.Components
{
    public struct VelocityComponent : IComponentData
    {
        public float3 Value;
    }
}