using Unity.Mathematics;
using UnityEngine;

namespace RedsAndBlues.Utils
{
    public static class PhysicsUtility
    {
        public static bool DoCirclesOverlap
        (
            float3 position1, float radius1,
            float3 position2, float radius2,
            float overlapError,
            out float overlapAmount
        )
        {
            float maxOverlap = radius1 + radius2;
            float dx = position2.x - position1.x;
            float dy = position1.y - position2.y;
            float sqrDistance = dx * dx + dy * dy;

            float sqrDiff = sqrDistance - maxOverlap * maxOverlap;

            bool collide = sqrDiff <= overlapError;

            overlapAmount = collide ? math.max(0, maxOverlap - math.sqrt(sqrDistance) - overlapError) : 0;

            return collide;
        }

        public static bool DoCirclesAndAABBOverlap
        (
            float3 circleCenter, float circleRadius,
            float3 aabbCenter, float2 aabbSize,
            float overlapError,
            out float3 closestPoint,
            out float overlapAmount
        )
        {
            var halfSize = aabbSize / 2f;

            var difference = circleCenter - aabbCenter;

            difference.x = math.clamp(difference.x, -halfSize.x, halfSize.x);
            difference.y = math.clamp(difference.y, -halfSize.y, halfSize.y);

            closestPoint = aabbCenter + difference;

            var sqrDistance = math.lengthsq(closestPoint - circleCenter);

            var sqrDiff = sqrDistance - circleRadius * circleRadius;

            bool collide = sqrDiff <= overlapError;

            if (IsPointInAABB(aabbCenter, aabbSize, circleCenter))
            {
                var normal = GetAABBNormalFromPoint(aabbCenter, aabbSize, circleCenter);

                var diff = circleCenter - aabbCenter - new float3(halfSize, 0) * normal;
                overlapAmount = math.length(diff * normal) + circleRadius;
            }
            else
            {
                overlapAmount = collide ? math.max(0, circleRadius - math.sqrt(sqrDistance) - overlapError) : 0;
            }

            return collide;
        }


        public static bool IsPointInAABB(float3 aabbCenter, float2 aabbSize, float3 point)
        {
            return point.x >= aabbCenter.x - aabbSize.x / 2f && point.x <= aabbCenter.x + aabbSize.x / 2f &&
                   point.y >= aabbCenter.y - aabbSize.y / 2f && point.y <= aabbCenter.y + aabbSize.y / 2f;
        }


        public static float3 GetAABBNormalFromPoint(float3 center, float2 size, float3 point)
        {
            var halfSize = size / 2f;

            float normalizedX = math.unlerp(center.x - halfSize.x, center.x + halfSize.x, point.x) * 2 - 1;
            float normalizedY = math.unlerp(center.y - halfSize.y, center.y + halfSize.y, point.y) * 2 - 1;

            if (math.abs(normalizedX) > math.abs(normalizedY))
            {
                return new float3(math.sign(normalizedX), 0, 0);
            }

            return new float3(0, math.sign(normalizedY), 0);
        }
    }
}