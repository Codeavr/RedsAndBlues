using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using RedsAndBlues.ECS.PhysicsEngine.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public class HandleBounceCollisionSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreate()
        {
            base.OnCreate();

            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _barrier.CreateCommandBuffer();

            Entities
                .WithAll<HandleCollisionWithBounceTag>()
                .ForEach(
                    (
                        Entity entity,
                        ref VelocityComponent velocity,
                        in Translation translation,
                        in CircleColliderComponent collider,
                        in DynamicBuffer<CollisionInfoElementData> collisions
                    ) =>
                    {
                        var pivot = float3.zero;

                        for (var index = 0; index < collisions.Length; index++)
                        {
                            var collision = collisions[index];
                            if (collision.AnotherLayer == CollisionLayer.Obstacle ||
                                collision.AnotherLayer == collider.Group)
                            {
                                pivot += collision.CollisionPivot;
                            }
                        }

                        pivot /= collisions.Length;

                        var direction = math.normalize(translation.Value.xy - pivot.xy);
                        
                        if (math.isnan(direction.x))
                        {
                            commandBuffer.RemoveComponent<HandleCollisionWithBounceTag>(entity);
                            return;
                        }

                        var velocity2d = velocity.Value.xy;
                        var velocity1d = math.length(velocity2d);

                        var reflection = math.reflect(velocity2d / velocity1d, direction) * velocity1d;

                        velocity.Value = new float3(reflection, velocity.Value.z);

                        commandBuffer.RemoveComponent<HandleCollisionWithBounceTag>(entity);
                    }).Run();
        }
    }
}