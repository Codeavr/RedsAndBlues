using System;
using UnityEngine;

namespace RedsAndBlues.Data
{
    [Serializable]
    public class GameConfig
    {
        [SerializeField]
        private float gameAreaWidth = 50;

        [SerializeField]
        private float gameAreaHeight = 50;

        [SerializeField]
        private float unitSpawnDelay = 500;

        [SerializeField]
        private int numUnitsToSpawn = 50;

        [SerializeField]
        private float minUnitRadius = .5f;

        [SerializeField]
        private float maxUnitRadius = 1f;

        [SerializeField]
        private float minUnitSpeed = 5f;

        [SerializeField]
        private float maxUnitSpeed = 10f;

        public float MaxUnitSpeed => maxUnitSpeed;

        public float MinUnitSpeed => minUnitSpeed;

        public float MaxUnitRadius => maxUnitRadius;

        public float MinUnitRadius => minUnitRadius;

        public int NumUnitsToSpawn => numUnitsToSpawn;

        public float UnitSpawnDelay => unitSpawnDelay;

        public float GameAreaHeight => gameAreaHeight;

        public float GameAreaWidth => gameAreaWidth;
    }
}