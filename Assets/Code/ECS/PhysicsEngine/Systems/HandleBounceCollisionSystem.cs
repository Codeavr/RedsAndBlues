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
                .WithoutBurst()
                .WithAll<HandleCollisionWithBounceTag>()
                .ForEach(
                    (
                        Entity entity,
                        ref MovementDirectionComponent direction,
                        in Translation translation,
                        in CircleColliderComponent collider,
                        in DynamicBuffer<CollisionInfoElementData> collisions
                    ) =>
                    {
                        var normal = float2.zero;

                        for (var index = 0; index < collisions.Length; index++)
                        {
                            var collision = collisions[index];
                            if (collision.AnotherLayer == CollisionLayer.Obstacle ||
                                collision.AnotherLayer == collider.Group)
                            {
                                normal += collision.AnotherNormal.xy;
                            }
                        }

                        normal /= collisions.Length;

                        Debug.DrawRay(translation.Value, new Vector3(normal.x, normal.y, 0), Color.green, 0.01f);

                        if (math.isnan(normal.x))
                        {
                            commandBuffer.RemoveComponent<HandleCollisionWithBounceTag>(entity);
                            return;
                        }

                        var reflection = math.reflect(direction.Value.xy, normal);

                        direction.Value = new float3(math.normalize(reflection), 0);

                        commandBuffer.RemoveComponent<HandleCollisionWithBounceTag>(entity);
                    }).Run();
        }
    }
}