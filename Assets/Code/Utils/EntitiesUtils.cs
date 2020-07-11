using System;
using System.Reflection;
using Unity.Entities;

namespace RedsAndBlues.Utils
{
    public static class EntitiesUtils
    {
        public static ComponentType GetWorldTimeType()
        {
            var assembly = Assembly.GetAssembly(typeof(Entity));
            return assembly.GetType("Unity.Entities.WorldTime");
        }
    }
}