using Unity.Mathematics;

namespace RedsAndBlues.ECS.PhysicsEngine.Utils
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

            overlapAmount = collide ? math.max(0, circleRadius - math.sqrt(sqrDistance) - overlapError) : 0;

            return collide;
        }
    }
}