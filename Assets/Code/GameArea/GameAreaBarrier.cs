using RedsAndBlues.Code.PhysicsEngine.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RedsAndBlues.Code.GameArea
{
    public class GameAreaBarrier
    {
        private const float BarrierWallWidth = 1f;

        public GameAreaBarrier(EntityManager manager, GameAreaSettings settings)
        {
            CreateBarrierWall(manager, // left barrier
                new float3(-settings.Width / 2f - BarrierWallWidth / 2f, 0, 0),
                new float2(BarrierWallWidth, settings.Height)
            );

            CreateBarrierWall(manager, // right barrier
                new float3(settings.Width / 2f + BarrierWallWidth / 2f, 0, 0),
                new float2(BarrierWallWidth, settings.Height)
            );

            CreateBarrierWall(manager, // top barrier
                new float3(0, settings.Height / 2f + BarrierWallWidth / 2f, 0),
                new float2(settings.Width, BarrierWallWidth)
            );

            CreateBarrierWall(manager, // bottom barrier
                new float3(0, -settings.Height / 2f - BarrierWallWidth / 2f, 0),
                new float2(settings.Width, BarrierWallWidth)
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