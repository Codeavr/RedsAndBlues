using System;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RedsAndBlues.GameArea
{
    public class GameAreaBarrier
    {
        private const float ExtendRatio = 1.2f;

        public Action RebuildEvent;

        public GameAreaSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public bool IsBuild { get; private set; }

        private GameAreaSettings _settings;
        private EntityManager _manager;

        public GameAreaBarrier(EntityManager manager, GameAreaSettings settings)
        {
            _manager = manager;
            _settings = settings;

            Rebuild();
        }

        public void Rebuild()
        {
            DeleteAllWalls();

            float wallWidth = math.min(_settings.Width, _settings.Height);

            CreateBarrierWall( // left barrier
                new float3(-_settings.Width / 2f - wallWidth / 2f, 0, 0),
                new float2(wallWidth, _settings.Height * ExtendRatio)
            );

            CreateBarrierWall( // right barrier
                new float3(_settings.Width / 2f + wallWidth / 2f, 0, 0),
                new float2(wallWidth, _settings.Height * ExtendRatio)
            );

            CreateBarrierWall( // top barrier
                new float3(0, _settings.Height / 2f + wallWidth / 2f, 0),
                new float2(_settings.Width * ExtendRatio, wallWidth)
            );

            CreateBarrierWall( // bottom barrier
                new float3(0, -_settings.Height / 2f - wallWidth / 2f, 0),
                new float2(_settings.Width * ExtendRatio, wallWidth)
            );

            IsBuild = true;

            RebuildEvent?.Invoke();
        }

        private void CreateBarrierWall(float3 position, float2 size)
        {
            var entity = _manager.CreateEntity(typeof(AABBColliderComponent), typeof(Translation));

            _manager.SetComponentData(entity, new Translation
            {
                Value = position
            });

            _manager.SetComponentData(entity, new AABBColliderComponent
            {
                Size = size
            });
        }

        private void DeleteAllWalls()
        {
            var wallsQuery = _manager.CreateEntityQuery(typeof(AABBColliderComponent));
            _manager.DestroyEntity(wallsQuery);
        }
    }
}