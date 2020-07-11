using System;
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

        public GameSaveSystem(SaveStorage storage)
        {
            _storage = storage;
        }

        public void Save()
        {
            var serializer = new WorldSerializer();
            var serializedWorld = serializer.Serialize(World.DefaultGameObjectInjectionWorld);

            _storage.Save(serializedWorld);
        }

        public void Load()
        {
            if (!HaveGameSave)
            {
                throw new Exception("Can't load unsaved game");
            }

            var gameSaver = new WorldSerializer();

            var loadedWorld = gameSaver.DeserializeWorld(_storage.Load());

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

        public void DeleteAll() => _storage.DeleteAll();
    }
}