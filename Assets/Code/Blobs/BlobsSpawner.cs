using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.General.Tags;
using RedsAndBlues.ECS.PhysicsEngine;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using RedsAndBlues.ECS.PhysicsEngine.Tags;
using RedsAndBlues.ECS.Rendering;
using RedsAndBlues.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RedsAndBlues.Blobs
{
    public class BlobsSpawner
    {
        private const int MaxFindPositionAttempts = 32;
        private EntityArchetype _ballArchetype;
        private EntityManager _manager;
        private int _spawnedCount;
        private EntityQuery _circlesQuery;

        public BlobsSpawner(EntityManager manager)
        {
            _manager = manager;
            _circlesQuery = _manager.CreateEntityQuery(typeof(CircleColliderComponent), typeof(Translation));

            _ballArchetype = _manager.CreateArchetype
            (
                typeof(Translation),
                typeof(CircleColliderComponent),
                typeof(CollisionInfoElementData),
                typeof(VelocityComponent),
                typeof(BlobPropertiesComponent),
                typeof(LocalToWorld),
                typeof(SpriteRenderComponent),
                typeof(Scale)
            );
        }

        public void SpawnBlob(BlobSpawnSettings spawnSettings)
        {
            var speed = Random.Range(spawnSettings.MinUnitSpeed, spawnSettings.MaxUnitSpeed);
            var radius = Random.Range(spawnSettings.MinUnitRadius, spawnSettings.MaxUnitRadius);
            bool isRed = _spawnedCount % 2 == 0;

            var area = new float2
            (
                spawnSettings.GameAreaSettings.Width - radius * 2,
                spawnSettings.GameAreaSettings.Height - radius * 2
            );

            var position = FindSuitablePosition(area, radius) + new float3(0, 0, spawnSettings.ZPosition);

            var entity = _manager.CreateEntity(_ballArchetype);

            _manager.SetComponentData(entity, new CircleColliderComponent
            {
                Radius = radius,
                Group = isRed ? CollisionLayer.Red : CollisionLayer.Blue
            });

            _manager.SetComponentData(entity, new VelocityComponent
            {
                Value = Quaternion.Euler(0, 0, Random.value * 360) * new Vector3(speed, 0f)
            });

            _manager.SetComponentData(entity, new Translation
            {
                Value = position
            });

            _manager.SetComponentData(entity, new BlobPropertiesComponent
            {
                DestroyRadius = spawnSettings.DestroyRadius
            });

            _manager.SetSharedComponentData(entity, new SpriteRenderComponent
            {
                Material = isRed ? spawnSettings.RedBlobMaterial : spawnSettings.BlueBlobMaterial
            });

            _spawnedCount++;
        }

        public void StartMovingAllTheBlobs()
        {
            var query = _manager.CreateEntityQuery(typeof(BlobPropertiesComponent));
            _manager.AddComponent(query, typeof(IsMovingTag));
        }

        private float3 FindSuitablePosition(float2 area, float circleRadius)
        {
            var entities = _circlesQuery.ToEntityArray(Allocator.TempJob);
            var colliders = _circlesQuery.ToComponentDataArray<CircleColliderComponent>(Allocator.TempJob);
            var translations = _circlesQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
            NativeArray<bool> isSuitable = new NativeArray<bool>(1, Allocator.TempJob);

            float3 position = default;

            for (int i = 0; i < MaxFindPositionAttempts; i++)
            {
                position = new float3(Random.Range(-.5f, .5f) * area.x, Random.Range(-.5f, .5f) * area.y, 0);

                isSuitable[0] = true;

                var job = new FindSuitablePositionJob
                {
                    Colliders = colliders,
                    Translations = translations,
                    Position = position,
                    Radius = circleRadius,
                    CollisionError = circleRadius * .1f,
                    IsSuitable = isSuitable
                };

                var jobHandle = job.Schedule();

                jobHandle.Complete();

                if (job.IsSuitable[0])
                {
                    break;
                }
            }

            entities.Dispose();
            colliders.Dispose();
            translations.Dispose();
            isSuitable.Dispose();

            return position;
        }

        [BurstCompile]
        private struct FindSuitablePositionJob : IJob
        {
            [ReadOnly]
            public NativeArray<CircleColliderComponent> Colliders;

            [ReadOnly]
            public NativeArray<Translation> Translations;

            [ReadOnly]
            public float3 Position;

            [ReadOnly]
            public float Radius;

            [ReadOnly]
            public float CollisionError;

            [WriteOnly]
            public NativeArray<bool> IsSuitable;

            public void Execute()
            {
                for (int index = 0; index < Colliders.Length; index++)
                {
                    if (PhysicsUtility.DoCirclesOverlap
                    (
                        Position, Radius,
                        Translations[index].Value, Colliders[index].Radius,
                        CollisionError, out _
                    ))
                    {
                        IsSuitable[0] = false;
                        return;
                    }
                }
            }
        }
    }
}