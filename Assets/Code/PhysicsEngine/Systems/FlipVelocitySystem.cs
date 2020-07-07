using RedsAndBlues.Code.PhysicsEngine.Components;
using RedsAndBlues.Code.PhysicsEngine.Tags;
using Unity.Entities;

namespace RedsAndBlues.Code.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public class FlipVelocitySystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<FlipVelocityTag>()
                .ForEach((Entity entity, ref VelocityComponent velocity) =>
                {
                    velocity.Value = -velocity.Value;
                    EntityManager.RemoveComponent<FlipVelocityTag>(entity);
                });
        }
    }
}