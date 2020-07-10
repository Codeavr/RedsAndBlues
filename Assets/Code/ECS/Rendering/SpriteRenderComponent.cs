using System;
using Unity.Entities;
using UnityEngine;

namespace RedsAndBlues.ECS.Rendering
{
    public struct SpriteRenderComponent : ISharedComponentData, IEquatable<SpriteRenderComponent>
    {
        public Material Material;

        public bool Equals(SpriteRenderComponent other)
        {
            return Equals(Material, other.Material);
        }

        public override bool Equals(object obj)
        {
            return obj is SpriteRenderComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Material != null ? Material.GetHashCode() : 0);
        }
    }
}