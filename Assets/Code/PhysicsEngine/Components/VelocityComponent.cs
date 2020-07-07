using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.Code.PhysicsEngine.Components
{
    public struct VelocityComponent : IComponentData
    {
        public float3 Value;
    }
}