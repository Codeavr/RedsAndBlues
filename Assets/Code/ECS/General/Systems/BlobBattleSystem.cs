﻿using JetBrains.Annotations;
using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.General.Tags;
using RedsAndBlues.ECS.PhysicsEngine;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;

namespace RedsAndBlues.ECS.General.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup)), UsedImplicitly]
    public class BlobBattleSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _barrier.CreateCommandBuffer();

            Entities.WithAll<HaveBattleTag>()
                .ForEach((
                    Entity entity,
                    ref CircleColliderComponent collider,
                    in BlobPropertiesComponent blobProperties,
                    in DynamicBuffer<CollisionInfoElementData> collisionInfo
                ) =>
                {
                    float radiusReductionAmount = 0;

                    for (var i = 0; i < collisionInfo.Length; i++)
                    {
                        radiusReductionAmount += collisionInfo[i].OverlapAmount;
                    }

                    float radius = collider.Radius - radiusReductionAmount;

                    if (radius < blobProperties.DestroyRadius)
                    {
                        commandBuffer.DestroyEntity(entity);

                        for (int i = 0; i < collisionInfo.Length; i++)
                        {
                            if (collisionInfo[i].AnotherLayer != CollisionLayer.Obstacle &&
                                collisionInfo[i].AnotherLayer != collider.Group)
                            {
                                commandBuffer.DestroyEntity(collisionInfo[i].AnotherEntity);
                            }
                        }
                    }
                    else
                    {
                        collider.Radius = radius;
                        commandBuffer.RemoveComponent<HaveBattleTag>(entity);
                    }
                }).Run();
        }
    }
}