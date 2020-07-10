using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ClearCollisionInfoBuffersSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CollisionInfoElementData>()
                .ForEach(entity => EntityManager.GetBuffer<CollisionInfoElementData>(entity).Clear());
        }
    }
}