using RedsAndBlues.Code.PhysicsEngine.Components;
using RedsAndBlues.Code.PhysicsEngine.Tags;
using RedsAndBlues.Code.PhysicsEngine.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace RedsAndBlues.Code.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class CircleToCircleCollisionDetectionSystem : JobComponentSystem
    {
        private EntityQuery _circlesGroup;
        private EndInitializationEntityCommandBufferSystem _barrier;

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

            _barrier = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

            _circlesGroup = GetEntityQuery(query);
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var entities = _circlesGroup.ToEntityArray(Allocator.TempJob);
            var colliders = _circlesGroup.ToComponentDataArray<CircleColliderComponent>(Allocator.TempJob);
            var translations = _circlesGroup.ToComponentDataArray<Translation>(Allocator.TempJob);

            var job = new CircleToCircleCollisionDetectionJob
            {
                Entities = entities,
                Colliders = colliders,
                Translations = translations,
                CommandBuffer = _barrier.CreateCommandBuffer().ToConcurrent()
            };

            var jobHandle = job.Schedule(colliders.Length, 32);

            jobHandle.Complete();

            entities.Dispose();
            colliders.Dispose();
            translations.Dispose();

            return jobHandle;
        }
    }

    [BurstCompile]
    public struct CircleToCircleCollisionDetectionJob : IJobParallelFor
    {
        private const float OverlapError = 0.025f;

        [ReadOnly]
        public NativeArray<Entity> Entities;

        [ReadOnly]
        public NativeArray<CircleColliderComponent> Colliders;

        [ReadOnly]
        public NativeArray<Translation> Translations;

        public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(int i)
        {
            var outerEntity = Entities[i];

            for (int j = i + 1; j < Colliders.Length; j++)
            {
                var innerEntity = Entities[j];

                if (PhysicsUtility.DoCirclesOverlap
                (
                    Translations[i].Value, Colliders[i].Radius,
                    Translations[j].Value, Colliders[j].Radius,
                    OverlapError, out float overlapAmount
                ))
                {
                    CommandBuffer.AppendToBuffer(i, outerEntity, new CollisionInfoElementData
                    (
                        innerEntity,
                        Translations[j].Value,
                        Colliders[j].Group,
                        overlapAmount
                    ));

                    CommandBuffer.AppendToBuffer(i, innerEntity, new CollisionInfoElementData
                    (
                        outerEntity,
                        Translations[i].Value,
                        Colliders[i].Group,
                        overlapAmount
                    ));

                    if (Colliders[i].Group == CollisionLayer.Obstacle ||
                        Colliders[j].Group == CollisionLayer.Obstacle ||
                        Colliders[j].Group == Colliders[i].Group)
                    {
                        CommandBuffer.AddComponent<HandleCollisionWithBounceTag>(i, outerEntity);
                        CommandBuffer.AddComponent<RigidbodyCollisionTag>(i, outerEntity);

                        CommandBuffer.AddComponent<HandleCollisionWithBounceTag>(i, innerEntity);
                        CommandBuffer.AddComponent<RigidbodyCollisionTag>(i, innerEntity);
                    }
                    else
                    {
                        CommandBuffer.AddComponent<HaveBattleTag>(i, outerEntity);
                        CommandBuffer.AddComponent<HaveBattleTag>(i, innerEntity);
                    }
                }
            }
        }
    }
}