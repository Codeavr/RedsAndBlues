using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.ECS.General.Components
{
    public struct MovementDirectionComponent : IComponentData
    {
        public float3 Value;
    }
}