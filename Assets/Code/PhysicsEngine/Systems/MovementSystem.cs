using JetBrains.Annotations;
using RedsAndBlues.Code.PhysicsEngine.Components;
using Unity.Entities;
using Unity.Transforms;

namespace RedsAndBlues.Code.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup)), UsedImplicitly]
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, in VelocityComponent velocity) =>
            {
                translation.Value += velocity.Value * deltaTime;
            }).Run();
        }
    }
}