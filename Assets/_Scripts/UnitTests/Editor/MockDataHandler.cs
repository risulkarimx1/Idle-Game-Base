using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services.DataFramework;

namespace _Scripts.UnitTests.Editor
{
    public class MockDataHandler : IDataHandler
    {
        private Dictionary<string, BaseData> DataStore = new Dictionary<string, BaseData>();

        public async UniTask<T> LoadAsync<T>(string fileName, string expectedKeyVersion) where T : BaseData, new()
        {
            if (DataStore.TryGetValue(fileName, out var data))
            {
                return (T)data;
            }

            return new T(); // Return a new instance if not found or version mismatch
        }

        public async UniTask SaveAsync(string dataIdentifier, BaseData baseData)
        {
            DataStore[dataIdentifier] = baseData;
        }
    }
}