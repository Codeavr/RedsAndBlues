using JetBrains.Annotations;
using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.General.Tags;
using Unity.Entities;
using Unity.Transforms;

namespace RedsAndBlues.ECS.General.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup)), UsedImplicitly]
    public class MovementSystem : SystemBase
    {
        private SimulationTime _time;

        public void Resolve(SimulationTime time)
        {
            _time = time;
        }
        
        protected override void OnUpdate()
        {
            float deltaTime = _time.DeltaTime;

            Entities
                .WithAll<IsMovingTag>()
                .ForEach((ref Translation translation, in SpeedComponent speed, in MovementDirectionComponent direction) =>
                    {
                        translation.Value += direction.Value * speed.Value * deltaTime;
                    }).Run();
        }
    }
}