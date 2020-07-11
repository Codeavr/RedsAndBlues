using RedsAndBlues.Blobs;
using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.General.Tags;
using RedsAndBlues.ECS.PhysicsEngine.Systems;
using RedsAndBlues.UI;
using Unity.Entities;
using UnityEngine;

namespace RedsAndBlues
{
    public class GameBehaviour : MonoBehaviour
    {
        private EntityManager _manager;
        private GameWinObserver _observer;
        private UiSaveLoadButtons _saveLoadButtons;
        private BlobsSpawner _blobsSpawner;
        private UiStartNewGameButton _startNewGameButton;

        public void Resolve
        (
            EntityManager manager,
            GameWinObserver observer,
            UiSaveLoadButtons saveLoadButtons,
            BlobsSpawner blobsSpawner,
            UiStartNewGameButton startNewGameButton)
        {
            _startNewGameButton = startNewGameButton;
            _blobsSpawner = blobsSpawner;
            _saveLoadButtons = saveLoadButtons;
            _observer = observer;
            _manager = manager;

            StartNewGame();
        }

        [ContextMenu(nameof(StartNewGame))]
        public void StartNewGame()
        {
            StopAllCoroutines(); // disable delayed spawning routine

            _saveLoadButtons.Interactable = false;
            _startNewGameButton.Interactable = false;
            _observer.IsEnabled = false;

            var blobsQuery = _manager.CreateEntityQuery(typeof(BlobPropertiesComponent));
            _manager.DestroyEntity(blobsQuery);

            _blobsSpawner.Reset();
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

            _saveLoadButtons.Interactable = true;
            _observer.IsEnabled = true;
            _startNewGameButton.Interactable = true;

            void StartUnitsMovement()
            {
                var query = _manager.CreateEntityQuery(typeof(BlobPropertiesComponent));
                _manager.AddComponent(query, typeof(IsMovingTag));
            }
        }

        private void SetPhysicsSystemsState(bool state)
        {
            _manager.World.GetExistingSystem<CircleToCircleCollisionDetectionSystem>().Enabled = state;
            _manager.World.GetExistingSystem<CircleToAABBCollisionDetectionSystem>().Enabled = state;
        }
    }
}