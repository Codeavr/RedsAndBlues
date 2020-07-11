using System;
using Unity.Entities;
using UnityEngine;

namespace RedsAndBlues.ECS.Rendering
{
    public struct SpriteRenderComponent : ISharedComponentData
    {
        public int MaterialId;
    }
}