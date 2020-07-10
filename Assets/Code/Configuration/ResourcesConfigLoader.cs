using System.Threading.Tasks;
using UnityEngine;

namespace RedsAndBlues.Configuration
{
    public class ResourcesConfigLoader<T>
    {
        private string _resourcePath;

        public ResourcesConfigLoader(string resourcePath)
        {
            _resourcePath = resourcePath;
        }

        public async Task<T> Load()
        {
            var request = Resources.LoadAsync<TextAsset>(_resourcePath);

            while (!request.isDone)
            {
                await Task.Yield();
            }

            var json = ((TextAsset) request.asset).text;

            return JsonUtility.FromJson<T>(json);
        }
    }
}