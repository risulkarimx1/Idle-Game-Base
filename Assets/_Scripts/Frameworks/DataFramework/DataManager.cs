using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Frameworks.InitializablesManager;
using UniRx;
using X1Frameworks.InitializablesManager;
using Zenject;

namespace Frameworks.DataFramework
{
    public interface IDataManager : IInitializable, IDisposable, IRequireInit
    {
        new bool InitFinished { get; }
        new void Initialize();
        UniTask SaveAsync<T>() where T : BaseData;
        UniTask SaveAllAsync();
        T Get<T>() where T : BaseData;
    }
    public class DataManager : IDataManager
    {
        public static string Key = "1234567890abcdef1234567890abcdef";
        public static string IV = "1234567890abcdef";
        
        [Inject] private IEncryptionService _encryptionService;

        [Inject] private IDataHandler _dataHandler;

        private readonly ReactiveProperty<bool> _isInitialized = new();
        public bool InitFinished => _isInitialized.Value;
        
        private GameInitManager _gameInitManager;

        private Dictionary<Type, BaseData> _typeToDataMatch = new();
        private Dictionary<Type, string> _typeToFileNameMatch = new();
        private List<Type> _derivedTypesCache = new();
        private MethodInfo _initializeDataTypeMethodCache = null;

        public void Initialize()
        {
            EnsureInitializedAsync().Forget();
        }

        private async UniTask EnsureInitializedAsync()
        {
            if (_isInitialized.Value) return;
            CacheReflectionResults();
            _typeToDataMatch.Clear();

            await UniTask.WhenAll(_derivedTypesCache.Select(async type =>
            {
                var genericMethod = _initializeDataTypeMethodCache.MakeGenericMethod(type);
                await (UniTask)genericMethod.Invoke(this, null);
            }));

            _isInitialized.Value = true;
            
        }
        private void CacheReflectionResults()
        {
            if (_derivedTypesCache == null || _derivedTypesCache.Count == 0)
            {
                var baseDataType = typeof(BaseData);
                _derivedTypesCache = Assembly.GetAssembly(baseDataType)
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseDataType))
                    .ToList();
            }

            if (_initializeDataTypeMethodCache == null)
            {
                _initializeDataTypeMethodCache = typeof(DataManager).GetMethod(nameof(InitializeDataType),
                    BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

       

        private UniTask<T> LoadAsync<T>() where T : BaseData, new()
        {
            var fileName = GetIdentifier(typeof(T));
            return _dataHandler.LoadAsync<T>(fileName, Key);
        }

        private async UniTask InitializeDataType<T>() where T : BaseData, new()
        {
            var data = await LoadAsync<T>();
            _typeToDataMatch.Add(typeof(T), data);
        }

        public UniTask SaveAsync<T>() where T : BaseData
        {
            BaseData data = _typeToDataMatch[typeof(T)];

            var fileName = GetIdentifier(typeof(T));
            return _dataHandler.SaveAsync(fileName, data);
        }

        public async UniTask SaveAllAsync()
        {
            await UniTask.WhenAll(_typeToDataMatch.Select(entry =>
            {
                var fileName = GetIdentifier(entry.Key);
                return _dataHandler.SaveAsync(fileName, entry.Value);
            }));
        }

        private string GetIdentifier(Type t)
        {
            if (_typeToFileNameMatch.ContainsKey(t))
                return _typeToFileNameMatch[t];

            var attribute = t.GetCustomAttribute<DataIdentifierAttribute>()?.Identifier ?? t.Name;
            var dataIdentifier = $"{attribute}_v{Key}.json";
            _typeToFileNameMatch.Add(t, dataIdentifier);

            return _typeToFileNameMatch[t];
        }

        public T Get<T>() where T : BaseData
        {
            return _typeToDataMatch[typeof(T)] as T;
        }

        public void Dispose()
        {
            _isInitialized.Dispose();
        }
    }
}