using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.Code.PhysicsEngine.Components
{
    public struct CollisionInfoElementData : IBufferElementData
    {
        public float OverlapAmount;
        public Entity AnotherEntity;
        public float3 CollisionPivot;
    }
}