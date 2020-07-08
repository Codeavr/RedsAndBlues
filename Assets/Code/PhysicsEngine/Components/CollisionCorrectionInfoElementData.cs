using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.Code.PhysicsEngine.Components
{
    public readonly struct CollisionInfoElementData : IBufferElementData
    {
        public readonly float OverlapAmount;
        public readonly Entity AnotherEntity;
        public readonly CollisionLayer AnotherLayer;
        public readonly float3 CollisionPivot;

        public CollisionInfoElementData(Entity anotherEntity, float3 collisionPivot, CollisionLayer anotherLayer, float overlapAmount)
        {
            OverlapAmount = overlapAmount;
            AnotherEntity = anotherEntity;
            AnotherLayer = anotherLayer;
            CollisionPivot = collisionPivot;
        }
    }
}