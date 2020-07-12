using Unity.Entities;
using Unity.Mathematics;

namespace RedsAndBlues.ECS.PhysicsEngine.Components
{
    public readonly struct CollisionInfoElementData : IBufferElementData
    {
        public readonly float OverlapAmount;
        public readonly Entity AnotherEntity;
        public readonly float3 AnotherNormal;
        public readonly CollisionLayer AnotherLayer;
        public readonly float3 CollisionPivot;

        public CollisionInfoElementData(Entity anotherEntity, float3 collisionPivot, CollisionLayer anotherLayer, float overlapAmount, float3 anotherNormal)
        {
            OverlapAmount = overlapAmount;
            AnotherNormal = anotherNormal;
            AnotherEntity = anotherEntity;
            AnotherLayer = anotherLayer;
            CollisionPivot = collisionPivot;
        }
    }
}