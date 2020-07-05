using System;
using RedAndBlues.Configuration;
using RedAndBlues.Data;
using RedAndBlues.Field;
using UnityEngine;

namespace RedAndBlues
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private string _configPath = "config";

        private async void Awake()
        {
            var configLoader = new ResourcesConfigLoader<ConfigRoot>(_configPath);

            GameConfig config = (await configLoader.Load()).GameConfig;

            var gameAreaSettings = new GameAreaSettings(config.GameAreaWidth, config.GameAreaHeight);

            FindObjectOfType<GameAreaView>().Initialize(gameAreaSettings);

            FindObjectOfType<CameraZoomToGameAreaBehaviour>().Initialize(gameAreaSettings);
        }
    }
}