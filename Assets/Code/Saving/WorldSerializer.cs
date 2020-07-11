using System;
using RedsAndBlues.Utils;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace RedsAndBlues.Saving
{
    public class WorldSerializer
    {
        public string Serialize(World world)
        {
            unsafe
            {
                var writer = new MemoryBinaryWriter();
                SerializeUtility.SerializeWorld(world.EntityManager, writer);

                var writtenBytes = new NativeArray<byte>(writer.Length, Allocator.Temp);

                UnsafeUtility.MemCpy((byte*) writtenBytes.GetUnsafePtr(), writer.Data, writer.Length);
                var text = Convert.ToBase64String(writtenBytes.ToArray());

                writtenBytes.Dispose();

                return text;
            }
        }

        public World DeserializeWorld(string base64)
        {
            var world = new World("Loaded world " + Time.time);
            var manager = world.EntityManager;

            var bytes = new NativeArray<byte>(Convert.FromBase64String(base64), Allocator.Temp);
            var transaction = manager.BeginExclusiveEntityTransaction();

            unsafe
            {
                var reader = new MemoryBinaryReader((byte*) bytes.GetUnsafePtr());
                SerializeUtility.DeserializeWorld(transaction, reader);
            }

            manager.EndExclusiveEntityTransaction();

            DestroyWorldTimeSingleton(world); // fix for ecs world time management

            bytes.Dispose();

            return world;
        }

        private void DestroyWorldTimeSingleton(World world)
        {
            var worldTimeType = EntitiesUtils.GetWorldTimeType();
            world.EntityManager.DestroyEntity(world.EntityManager.CreateEntityQuery(worldTimeType));
        }
    }
}