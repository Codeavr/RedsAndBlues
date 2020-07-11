using System;
using RedsAndBlues.Blobs;
using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.General.Tags;
using RedsAndBlues.ECS.PhysicsEngine.Systems;
using Unity.Entities;
using UnityEngine;

namespace RedsAndBlues
{
    public class GameBehaviour : MonoBehaviour
    {
        private EntityManager _manager;
        private World _world;
        private BlobsSpawner _blobsSpawner;
        private GameWinObserver _winObserver;

        public void Resolve
        (
            EntityManager manager,
            World world,
            BlobsSpawner blobsSpawner,
            GameWinObserver winObserver)
        {
            _manager = manager;
            _world = world;
            _blobsSpawner = blobsSpawner;
            _winObserver = winObserver;

            StartNewGame();
        }

        public void StartNewGame()
        {
            StopAllCoroutines(); // disable delayed spawning routine

            var blobsQuery = _manager.CreateEntityQuery(typeof(BlobPropertiesComponent));
            _manager.DestroyEntity(blobsQuery);

            _winObserver.IsEnabled = false;
            SetupBlobs();
        }

        private void SetupBlobs()
        {
            SetPhysicsSystemsState(false);
            _blobsSpawner.ReachedCapacityEvent += OnBlobsSpawnerReachedCapacity;
            StartCoroutine(_blobsSpawner.StartDelayedSpawningRoutine());
        }

        private void OnBlobsSpawnerReachedCapacity()
        {
            SetPhysicsSystemsState(true);
            StartUnitsMovement();
            _winObserver.IsEnabled = true;

            void StartUnitsMovement()
            {
                var query = _manager.CreateEntityQuery(typeof(BlobPropertiesComponent));
                _manager.AddComponent(query, typeof(IsMovingTag));
            }
        }

        private void SetPhysicsSystemsState(bool state)
        {
            _world.GetExistingSystem<CircleToCircleCollisionDetectionSystem>().Enabled = state;
            _world.GetExistingSystem<CircleToAABBCollisionDetectionSystem>().Enabled = state;
        }
    }
}