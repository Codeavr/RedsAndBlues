using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.ECS.General.Components
{
    public struct VelocityComponent : IComponentData
    {
        public float3 Value;
    }
}