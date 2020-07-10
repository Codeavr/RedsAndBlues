﻿using JetBrains.Annotations;
using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.General.Tags;
using Unity.Entities;
using Unity.Transforms;

namespace RedsAndBlues.ECS.General.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup)), UsedImplicitly]
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.WithAll<IsMovingTag>().ForEach((ref Translation translation, in VelocityComponent velocity) =>
            {
                translation.Value += velocity.Value * deltaTime;
            }).Run();
        }
    }
}