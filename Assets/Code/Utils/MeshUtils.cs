using UnityEngine;

namespace RedsAndBlues.Utils
{
    public static class MeshUtils
    {
        public static Mesh CreateQuad()
        {
            return new Mesh
            {
                vertices = new[]
                {
                    new Vector3(-0.5F, -0.5F, -0.5F),
                    new Vector3(0.5F, -0.5F, -0.5F),
                    new Vector3(-0.5F, 0.5F, -0.5F),
                    new Vector3(0.5F, 0.5F, -0.5F)
                },

                triangles = new[]
                {
                    0, 2, 1,
                    2, 3, 1
                },

                normals = new[]
                {
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward
                },

                uv = new[]
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1)
                }
            };
        }
    }
}