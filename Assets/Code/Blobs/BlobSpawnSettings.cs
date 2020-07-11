using RedsAndBlues.GameArea;
using Unity.Collections;
using UnityEngine;

namespace RedsAndBlues.Blobs
{
    public readonly struct BlobsSpawningSettings
    {
        public readonly int Capacity;
        public readonly float Delay;
        public readonly float MinUnitRadius;
        public readonly float MaxUnitRadius;
        public readonly float MinUnitSpeed;
        public readonly float MaxUnitSpeed;
        public readonly float ZPosition;
        public readonly float DestroyRadius;
        public readonly GameAreaSettings GameAreaSettings;
        public readonly Material RedBlobMaterial;
        public readonly Material BlueBlobMaterial;

        public BlobsSpawningSettings
        (
            int capacity, float delay,
            float minUnitRadius, float maxUnitRadius,
            float minUnitSpeed, float maxUnitSpeed,
            float destroyRadius,
            float zPosition,
            GameAreaSettings gameAreaSettings,
            Material redBlobMaterial, Material blueBlobMaterial)
        {
            Capacity = capacity;
            Delay = delay;
            MinUnitRadius = minUnitRadius;
            MaxUnitRadius = maxUnitRadius;
            MinUnitSpeed = minUnitSpeed;
            MaxUnitSpeed = maxUnitSpeed;
            DestroyRadius = destroyRadius;
            ZPosition = zPosition;
            GameAreaSettings = gameAreaSettings;
            RedBlobMaterial = redBlobMaterial;
            BlueBlobMaterial = blueBlobMaterial;
        }
    }
}