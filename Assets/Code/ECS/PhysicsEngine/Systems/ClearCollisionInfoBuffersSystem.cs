using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ClearCollisionInfoBuffersSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref DynamicBuffer<CollisionInfoElementData> buffer) => buffer.Clear()).ScheduleParallel();
        }
    }
}