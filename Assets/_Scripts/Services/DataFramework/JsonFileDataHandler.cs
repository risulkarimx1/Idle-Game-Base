using System.IO;
using Cysharp.Threading.Tasks;
using GameCode.Init;
using Newtonsoft.Json;
using Services.LogFramework;
using UnityEngine;
using Debug = Services.LogFramework.Debug;

namespace Services.DataFramework
{
    public class JsonFileDataHandler : BaseDataHandler, IDataHandler
    {
        private readonly IEncryptionService _encryptionService;
        private readonly string _directoryPath;
        public bool IsDirty { get; private set; }

        public JsonFileDataHandler(IEncryptionService service)
        {
            IV = GameConfig.GetInstance().DataInitVector;
            Key = GameConfig.GetInstance().DataKey;

            _directoryPath = Path.Combine(Application.persistentDataPath, GameConfig.GetInstance().DataKey);
            _encryptionService = service;
            IsDirty = false;
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        public async UniTask SaveAsync(string fileName, BaseData data)
        {
            var filePath = Path.Combine(_directoryPath, $"{fileName}");

            var serializedData = JsonConvert.SerializeObject(data);

            var encryptedData = new EncryptedData
            {
                Data = await _encryptionService.EncryptStringAsync(serializedData),
                KeyVersion = Key
            };

            var encryptedDataAsJson = JsonConvert.SerializeObject(encryptedData, Formatting.Indented);

            await File.WriteAllTextAsync(filePath, encryptedDataAsJson);
            Debug.Log($"Data forced saved to {filePath}", LogContext.DataManager);
        }


        public async UniTask<T> LoadAsync<T>(string fileName, string expectedKeyVersion) where T : BaseData, new()
        {
            var filePath = Path.Combine(_directoryPath, fileName);

            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                var encryptedData = JsonConvert.DeserializeObject<EncryptedData>(json);


                if (encryptedData.KeyVersion != expectedKeyVersion)
                {
                    Debug.Log($"Key version mismatch. Migrating data to version {expectedKeyVersion}.",
                        LogContext.DataManager);
                    var decryptedData = await _encryptionService.DecryptString(encryptedData.Data);
                    var dataObject = JsonConvert.DeserializeObject<T>(decryptedData);
                    Key = expectedKeyVersion;
                    await SaveAsync(fileName, dataObject);
                    return dataObject;
                }
                else
                {
                    var decryptedData = await _encryptionService.DecryptString(encryptedData.Data);
                    var dataObject = JsonConvert.DeserializeObject<T>(decryptedData);
                    return dataObject;
                }
            }

            // The file does not exist, so create a new instance of T, save it, and log a message indicating creation of a new file.
            T newData = new T();
            await SaveAsync(fileName, newData);
            Debug.Log($"File not found: {filePath}. Created a new file with default data.", LogContext.DataManager);
            return newData;
        }
    }
}