using System;
using RedsAndBlues.GameArea;
using RedsAndBlues.Saving;
using RedsAndBlues.Utils;
using Unity.Entities;
using UnityEngine;

namespace RedsAndBlues
{
    public class GameSaveSystem
    {
        public bool HaveGameSave => _storage.HaveSavedData;

        private SaveStorage _storage;
        private GameAreaBarrier _barrier;
        private SimulationTime _time;

        public GameSaveSystem(SaveStorage storage, GameAreaBarrier barrier, SimulationTime time)
        {
            _time = time;
            _barrier = barrier;
            _storage = storage;
        }

        public void Save()
        {
            var serializer = new WorldSerializer();
            var serializedWorld = serializer.Serialize(World.DefaultGameObjectInjectionWorld);

            var gameSaveData = new GameSaveData
            {
                SerializedWorld = serializedWorld,
                SimulationTime = _time,
                GameAreaSettings = _barrier.Settings,
                TimeStamp = DateTime.Now.Ticks
            };

            var json = JsonUtility.ToJson(gameSaveData);

            _storage.Save(json);
        }

        public void Load()
        {
            if (!HaveGameSave)
            {
                throw new Exception("Can't load unsaved game");
            }

            var json = _storage.Load();

            var saveData = JsonUtility.FromJson<GameSaveData>(json);

            RestoreWorld(saveData.SerializedWorld);
            RestoreGameArea(saveData.GameAreaSettings);
            _time.Restore(saveData.SimulationTime);
        }

        public void DeleteAll() => _storage.DeleteAll();

        private void RestoreGameArea(GameAreaSettings settings)
        {
            _barrier.Settings = settings;
            _barrier.Rebuild();
        }

        private static void RestoreWorld(string saveDataSerializedWorld)
        {
            var worldSerializer = new WorldSerializer();

            var loadedWorld = worldSerializer.DeserializeWorld(saveDataSerializedWorld);

            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var query = manager.CreateEntityQuery(new EntityQueryDesc
            {
                None = new[]
                {
                    EntitiesUtils.GetWorldTimeType()
                }
            });

            manager.DestroyEntity(query);
            manager.MoveEntitiesFrom(loadedWorld.EntityManager);

            loadedWorld.Dispose();
        }
    }
}