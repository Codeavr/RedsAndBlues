using System.Collections.Generic;
using RedsAndBlues.Blobs;
using RedsAndBlues.Configuration;
using RedsAndBlues.Data;
using RedsAndBlues.ECS.Rendering;
using RedsAndBlues.GameArea;
using RedsAndBlues.UI;
using Unity.Entities;
using UnityEngine;

namespace RedsAndBlues
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private string _configPath = "config";

        [SerializeField]
        private Material _redBlobMaterial;

        [SerializeField]
        private Material _blueBlobMaterial;

        private List<ITickable> _tickables = new List<ITickable>();

        private bool _finishedInitialization;

        private async void Awake()
        {
            var configLoader = new ResourcesConfigLoader<ConfigRoot>(_configPath);

            var world = World.DefaultGameObjectInjectionWorld;
            var manager = world.EntityManager;

            var config = (await configLoader.Load()).GameConfig;

            var gameAreaSettings = new GameAreaSettings(config.GameAreaWidth, config.GameAreaHeight);

            var blobsSpawnerSettings = new BlobsSpawningSettings
            (
                config.NumUnitsToSpawn, config.UnitSpawnDelay / 1000f,
                config.MinUnitRadius, config.MaxUnitRadius,
                config.MinUnitSpeed, config.MaxUnitSpeed,
                0.2f, -1, gameAreaSettings, _redBlobMaterial, _blueBlobMaterial
            );

            // DI
            manager.World.GetExistingSystem<InstancedSpriteRendererSystem>()
                .RegisterMaterial(_redBlobMaterial.name.GetHashCode(), _redBlobMaterial);
            manager.World.GetExistingSystem<InstancedSpriteRendererSystem>()
                .RegisterMaterial(_blueBlobMaterial.name.GetHashCode(), _blueBlobMaterial);

            var _ = new GameAreaBarrier(manager, gameAreaSettings);

            FindObjectOfType<GameAreaView>().Resolve(gameAreaSettings);

            FindObjectOfType<CameraZoomToGameAreaBehaviour>().Resolve(gameAreaSettings);

            var blobsSpawner = new BlobsSpawner(manager, blobsSpawnerSettings);

            var winObserver = new GameWinObserver(manager);
            _tickables.Add(winObserver);

            var gameSaveSystem = new GameSaveSystem(new SaveStorage());

            var uiSaveLoadButtons = FindObjectOfType<UiSaveLoadButtons>();
            uiSaveLoadButtons.Resolve(gameSaveSystem);

            var startNewGameButton = FindObjectOfType<UiStartNewGameButton>();
            var gameBehaviour = FindObjectOfType<GameBehaviour>();
            gameBehaviour.Resolve(manager, winObserver, uiSaveLoadButtons, blobsSpawner, startNewGameButton);
            startNewGameButton.Resolve(gameBehaviour);

            FindObjectOfType<UiPopupsController>().Resolve(winObserver, gameBehaviour);

            FindObjectOfType<UiGameInfo>().Resolve(winObserver);

            FindObjectOfType<GizmosDebugger>().Resolve(manager);

            _finishedInitialization = true;
        }

        private void Update()
        {
            if (!_finishedInitialization) return;

            for (var i = 0; i < _tickables.Count; i++)
            {
                if (_tickables[i].IsEnabled)
                {
                    _tickables[i].Tick();
                }
            }
        }
    }
}