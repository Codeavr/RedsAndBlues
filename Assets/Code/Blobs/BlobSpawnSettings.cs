using RedsAndBlues.Code.GameArea;
using UnityEngine;

namespace RedsAndBlues.Code.Blobs
{
    public readonly struct BlobSpawnSettings
    {
        public readonly float MinUnitRadius;
        public readonly float MaxUnitRadius;
        public readonly float MinUnitSpeed;
        public readonly float MaxUnitSpeed;
        public readonly float ZPosition;
        public readonly float DestroyRadius;
        public readonly GameAreaSettings GameAreaSettings;
        public readonly Mesh Mesh;
        public readonly Material Material;

        public BlobSpawnSettings
        (
            float minUnitRadius, float maxUnitRadius,
            float minUnitSpeed, float maxUnitSpeed,
            float destroyRadius,
            float zPosition,
            GameAreaSettings gameAreaSettings, Mesh mesh, Material material)
        {
            MinUnitRadius = minUnitRadius;
            MaxUnitRadius = maxUnitRadius;
            MinUnitSpeed = minUnitSpeed;
            MaxUnitSpeed = maxUnitSpeed;
            GameAreaSettings = gameAreaSettings;
            Mesh = mesh;
            Material = material;
            DestroyRadius = destroyRadius;
            ZPosition = zPosition;
        }
    }
}