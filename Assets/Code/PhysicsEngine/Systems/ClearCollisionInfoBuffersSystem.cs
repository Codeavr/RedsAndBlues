using RedsAndBlues.Code.PhysicsEngine.Components;
using Unity.Entities;

namespace RedsAndBlues.Code.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ClearCollisionInfoBuffersSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
        }

        protected override void OnUpdate()
        {
            Entities
                .WithAll<CollisionInfoElementData>()
                .ForEach(entity => EntityManager.GetBuffer<CollisionInfoElementData>(entity).Clear());
        }
    }
}