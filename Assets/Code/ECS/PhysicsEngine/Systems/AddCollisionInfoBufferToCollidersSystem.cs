using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup)), UpdateBefore(typeof(ClearCollisionInfoBuffersSystem))]
    public class AddCollisionInfoBufferToCollidersSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CircleColliderComponent>()
                .WithNone<CollisionInfoElementData>()
                .ForEach(entity => EntityManager.AddBuffer<CollisionInfoElementData>(entity));
        }
    }
}