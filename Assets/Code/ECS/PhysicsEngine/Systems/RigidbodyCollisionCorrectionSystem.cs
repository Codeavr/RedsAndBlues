﻿using JetBrains.Annotations;
using RedsAndBlues.ECS.General.Systems;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using RedsAndBlues.ECS.PhysicsEngine.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup)), UpdateBefore(typeof(MovementSystem)), UsedImplicitly]
    public class RigidbodyCollisionCorrectionSystem : SystemBase
    {
        private const float OvershootValue = 0.025f;

        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _barrier.CreateCommandBuffer();

            Entities
                .WithAll<RigidbodyCollisionTag>()
                .ForEach(
                    (
                        Entity entity,
                        ref Translation translation,
                        in DynamicBuffer<CollisionInfoElementData> collisionInfos) =>
                    {
                        var position = translation.Value;

                        for (var i = 0; i < collisionInfos.Length; i++)
                        {
                            var offset = collisionInfos[i].OverlapAmount * 0.5f;
                            var direction = collisionInfos[i].AnotherNormal;

                            position += direction * offset;
                        }

                        if (!math.isnan(position.x))
                        {
                            translation.Value = position;
                        }

                        commandBuffer.RemoveComponent<RigidbodyCollisionTag>(entity);
                    }).Run();
        }
    }
}