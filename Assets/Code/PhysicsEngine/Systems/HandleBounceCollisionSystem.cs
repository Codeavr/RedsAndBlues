using RedsAndBlues.Code.PhysicsEngine.Components;
using RedsAndBlues.Code.PhysicsEngine.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RedsAndBlues.Code.PhysicsEngine.Systems
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

                        var direction = math.normalize(translation.Value - pivot);

                        velocity.Value = math.reflect(velocity.Value, direction);

                        commandBuffer.RemoveComponent<HandleCollisionWithBounceTag>(entity);
                    }).Run();
        }
    }
}