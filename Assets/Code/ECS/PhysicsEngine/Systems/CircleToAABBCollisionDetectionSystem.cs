using RedsAndBlues.ECS.PhysicsEngine.Components;
using RedsAndBlues.ECS.PhysicsEngine.Tags;
using RedsAndBlues.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace RedsAndBlues.ECS.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class CircleToAABBCollisionDetectionSystem : JobComponentSystem
    {
        private EndInitializationEntityCommandBufferSystem _barrier;
        private EntityQuery _circlesGroup;
        private EntityQuery _aabbGroup;

        protected override void OnCreate()
        {
            var query = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    typeof(CircleColliderComponent),
                    typeof(Translation)
                }
            };

            _circlesGroup = GetEntityQuery(query);

            query = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    typeof(AABBColliderComponent),
                    typeof(Translation)
                }
            };

            _aabbGroup = GetEntityQuery(query);

            _barrier = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var circleEntities = _circlesGroup.ToEntityArray(Allocator.TempJob);
            var circleColliders = _circlesGroup.ToComponentDataArray<CircleColliderComponent>(Allocator.TempJob);
            var circleTranslations = _circlesGroup.ToComponentDataArray<Translation>(Allocator.TempJob);

            var aabbEntities = _aabbGroup.ToEntityArray(Allocator.TempJob);
            var aabbColliders = _aabbGroup.ToComponentDataArray<AABBColliderComponent>(Allocator.TempJob);
            var aabbTranslations = _aabbGroup.ToComponentDataArray<Translation>(Allocator.TempJob);

            var job = new CircleToCircleCollisionDetectionJob
            {
                CircleEntities = circleEntities,
                CircleColliders = circleColliders,
                CircleTranslations = circleTranslations,
                AABBEntities = aabbEntities,
                AABBColliders = aabbColliders,
                AABBTranslations = aabbTranslations,
                CommandBuffer = _barrier.CreateCommandBuffer().ToConcurrent()
            };

            var jobHandle = job.Schedule(circleEntities.Length, 32);

            jobHandle.Complete();

            circleEntities.Dispose();
            circleColliders.Dispose();
            circleTranslations.Dispose();

            aabbEntities.Dispose();
            aabbColliders.Dispose();
            aabbTranslations.Dispose();

            return jobHandle;
        }

        [BurstCompile]
        public struct CircleToCircleCollisionDetectionJob : IJobParallelFor
        {
            private const float OverlapError = 0.025f;

            [ReadOnly]
            public NativeArray<Entity> CircleEntities;

            [ReadOnly]
            public NativeArray<CircleColliderComponent> CircleColliders;

            [ReadOnly]
            public NativeArray<Translation> CircleTranslations;

            [ReadOnly]
            public NativeArray<Entity> AABBEntities;

            [ReadOnly]
            public NativeArray<AABBColliderComponent> AABBColliders;

            [ReadOnly]
            public NativeArray<Translation> AABBTranslations;

            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(int jobIndex)
            {
                var circleRadius = CircleColliders[jobIndex].Radius;
                var circleTranslation = CircleTranslations[jobIndex].Value;
                var circleEntity = CircleEntities[jobIndex];

                for (int i = 0; i < AABBColliders.Length; i++)
                {
                    if (PhysicsUtility.DoCirclesAndAABBOverlap
                    (
                        circleTranslation, circleRadius,
                        AABBTranslations[i].Value, AABBColliders[i].Size,
                        OverlapError, out float3 closestPoint, out float overlapAmount
                    ))
                    {
                        var normal = PhysicsUtility.GetAABBNormalFromPoint
                        (
                            AABBTranslations[i].Value,
                            AABBColliders[i].Size,
                            circleTranslation
                        );

                        CommandBuffer.AppendToBuffer(jobIndex, circleEntity, new CollisionInfoElementData
                        (
                            AABBEntities[i],
                            closestPoint,
                            CollisionLayer.Obstacle,
                            overlapAmount,
                            normal
                        ));

                        CommandBuffer.AddComponent<HandleCollisionWithBounceTag>(jobIndex, circleEntity);
                        CommandBuffer.AddComponent<RigidbodyCollisionTag>(jobIndex, circleEntity);
                    }
                }
            }
        }
    }
}