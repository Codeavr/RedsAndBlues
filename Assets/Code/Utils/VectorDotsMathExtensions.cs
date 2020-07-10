using Unity.Mathematics;
using UnityEngine;

namespace RedsAndBlues.Utils
{
    public static class VectorDotsMathExtensions
    {
        public static float3 ToF3(this Vector3 vec)
        {
            return new float3(vec.x, vec.y, vec.z);
        }
    }
}