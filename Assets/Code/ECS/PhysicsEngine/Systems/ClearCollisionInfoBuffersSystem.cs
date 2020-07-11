using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ClearCollisionInfoBuffersSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var job = Entities.ForEach((ref DynamicBuffer<CollisionInfoElementData> buffer) => buffer.Clear())
                .ScheduleParallel(Dependency);
            job.Complete();
        }
    }
}