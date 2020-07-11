using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RedsAndBlues.GameArea
{
    public class GameAreaBarrier
    {
        private const float ExtendRatio = 1.2f;

        public GameAreaBarrier(EntityManager manager, GameAreaSettings settings)
        {
            float wallWidth = math.min(settings.Width, settings.Height);

            CreateBarrierWall(manager, // left barrier
                new float3(-settings.Width / 2f - wallWidth / 2f, 0, 0),
                new float2(wallWidth, settings.Height * ExtendRatio)
            );

            CreateBarrierWall(manager, // right barrier
                new float3(settings.Width / 2f + wallWidth / 2f, 0, 0),
                new float2(wallWidth, settings.Height * ExtendRatio)
            );

            CreateBarrierWall(manager, // top barrier
                new float3(0, settings.Height / 2f + wallWidth / 2f, 0),
                new float2(settings.Width * ExtendRatio, wallWidth)
            );

            CreateBarrierWall(manager, // bottom barrier
                new float3(0, -settings.Height / 2f - wallWidth / 2f, 0),
                new float2(settings.Width * ExtendRatio, wallWidth)
            );
        }

        private static void CreateBarrierWall(EntityManager manager, float3 position, float2 size)
        {
            var entity = manager.CreateEntity(typeof(AABBColliderComponent), typeof(Translation));

            manager.SetComponentData(entity, new Translation
            {
                Value = position
            });

            manager.SetComponentData(entity, new AABBColliderComponent
            {
                Size = size
            });
        }
    }
}