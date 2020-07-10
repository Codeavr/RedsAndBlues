using RedsAndBlues.ECS.PhysicsEngine;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using RedsAndBlues.ECS.PhysicsEngine.Tags;
using RedsAndBlues.ECS.Rendering;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RedsAndBlues.Blobs
{
    public class BlobsSpawner
    {
        private EntityArchetype _ballArchetype;
        private EntityManager _manager;
        private int _spawnedCount;

        public BlobsSpawner(EntityManager manager)
        {
            _manager = manager;
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
            var position = new float3
            (
                Random.Range(-.5f, .5f) * (spawnSettings.GameAreaSettings.Width - radius * 2),
                Random.Range(-.5f, .5f) * (spawnSettings.GameAreaSettings.Height - radius * 2),
                spawnSettings.ZPosition
            );

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
                Material = isRed ? spawnSettings.RedBlobMaterial : spawnSettings.BlueBlobMaterial,
                Mesh = spawnSettings.Mesh
            });

            _spawnedCount++;
        }

        public void StartMovingAllTheBlobs()
        {
            var query = _manager.CreateEntityQuery(typeof(BlobPropertiesComponent));
            _manager.AddComponent(query, typeof(IsMovingTag));
        }
    }
}