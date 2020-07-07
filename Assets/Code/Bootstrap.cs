﻿using RedsAndBlues.Code.Blobs;
using RedsAndBlues.Code.Configuration;
using RedsAndBlues.Code.Data;
using RedsAndBlues.Code.GameArea;
using RedsAndBlues.Code.PhysicsEngine.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RedsAndBlues.Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private string _configPath = "config";

        [SerializeField]
        private Mesh _blobMesh;

        [SerializeField]
        private Material _blobMaterial;

        private GameConfig _config;
        private EntityManager _manager;
        private BlobsSpawner _blobsSpawner;
        private GameAreaSettings _gameAreaSettings;
        private BlobSpawnSettings _blobSpawnSettings;
        private GameAreaBarrier _gameAreaBarrier;

        private async void Awake()
        {
            var configLoader = new ResourcesConfigLoader<ConfigRoot>(_configPath);

            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _config = (await configLoader.Load()).GameConfig;

            SetupGameArea();
            SetupBlobs();
        }


        private void SetupBlobs()
        {
            _blobSpawnSettings = new BlobSpawnSettings
            (
                _config.MinUnitRadius, _config.MaxUnitRadius,
                _config.MinUnitSpeed, _config.MaxUnitSpeed,
                0.2f, -1, _gameAreaSettings,
                _blobMesh, _blobMaterial
            );

            _blobsSpawner = new BlobsSpawner(_manager);

            for (int i = 0; i < _config.NumUnitsToSpawn; i++)
            {
                _blobsSpawner.SpawnBlob(_blobSpawnSettings);
            }
        }

        private void SetupGameArea()
        {
            _gameAreaSettings = new GameAreaSettings(_config.GameAreaWidth, _config.GameAreaHeight);
            _gameAreaBarrier = new GameAreaBarrier(_manager, _gameAreaSettings);
            FindObjectOfType<GameAreaView>().Initialize(_gameAreaSettings);
            FindObjectOfType<CameraZoomToGameAreaBehaviour>().Initialize(_gameAreaSettings);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            var query = _manager.CreateEntityQuery(new ComponentType[]
            {
                typeof(CollisionInfoElementData), typeof(CircleColliderComponent), typeof(Translation)
            });

            var entities = query.ToEntityArray(Allocator.TempJob);

            for (int i = 0; i < entities.Length; i++)
            {
                bool isColliding = _manager.GetBuffer<CollisionInfoElementData>(entities[i]).Length > 0;
                float radius = _manager.GetComponentData<CircleColliderComponent>(entities[i]).Radius;

                var position = _manager.GetComponentData<Translation>(entities[i]).Value;

                Gizmos.color = isColliding ? Color.red : Color.green;

                Gizmos.DrawWireSphere(new Vector3(position.x, position.y, 0), radius);
            }

            entities.Dispose();
        }
    }
}