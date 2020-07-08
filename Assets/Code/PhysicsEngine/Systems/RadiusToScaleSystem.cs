using JetBrains.Annotations;
using RedsAndBlues.Code.PhysicsEngine.Components;
using Unity.Entities;
using Unity.Transforms;

namespace RedsAndBlues.Code.PhysicsEngine.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup)), UsedImplicitly]
    public class RadiusToScaleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Scale scale, in CircleColliderComponent collider) =>
            {
                scale.Value = collider.Radius * 2f;
            }).Run();
        }
    }
}