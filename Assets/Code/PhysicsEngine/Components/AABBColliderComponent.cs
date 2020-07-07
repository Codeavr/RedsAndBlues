using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.Code.PhysicsEngine.Components
{
    public struct AABBColliderComponent : IComponentData
    {
        public float2 Size;
    }
}