using System.Collections.Generic;
using RedsAndBlues.Code.PhysicsEngine.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RedsAndBlues.Code.Rendering
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class InstancedSpriteRendererSystem : ComponentSystem
    {
        private const int MatricesCapacity = 1023; // max 1023

        private readonly Matrix4x4[] _matrices = new Matrix4x4[MatricesCapacity];
        private EntityQuery _spriteGroup;

        private List<SpriteRenderComponent> _uniqueComponents = new List<SpriteRenderComponent>();
        private Mesh mesh;

        protected override void OnCreate()
        {
            base.OnCreate();

            var query = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    typeof(SpriteRenderComponent), typeof(LocalToWorld)
                }
            };

            _spriteGroup = GetEntityQuery(query);

            mesh = CreateQuad();
        }

        protected override void OnUpdate()
        {
            EntityManager.GetAllUniqueSharedComponentData(_uniqueComponents);
            foreach (var renderInfo in _uniqueComponents)
            {
                if (renderInfo.Material == null) continue;

                if (!renderInfo.Material.enableInstancing)
                {
                    renderInfo.Material.enableInstancing = true;
                }

                _spriteGroup.SetSharedComponentFilter(renderInfo);
                var transforms = _spriteGroup.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);

                int beginIndex = 0;
                while (beginIndex < transforms.Length)
                {
                    int length = math.min(_matrices.Length, transforms.Length - beginIndex);
                    DumpFloat4X4ToMatrix4X4Array(transforms, beginIndex, length);

                    Graphics.DrawMeshInstanced(renderInfo.Mesh, 0, renderInfo.Material, _matrices, length);

                    beginIndex += length;
                }

                transforms.Dispose();
            }

            _uniqueComponents.Clear();
        }

        private void DumpFloat4X4ToMatrix4X4Array(NativeArray<LocalToWorld> transforms, int beginIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _matrices[i] = transforms[beginIndex + i].Value;
            }
        }

        private static Mesh CreateQuad()
        {
            return new Mesh
            {
                vertices = new[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(1, 1, 0)
                }
            };
        }
    }
}