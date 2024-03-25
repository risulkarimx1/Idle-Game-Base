using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCode.Init;
using Services.LogFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Debug = Services.LogFramework.Debug;

namespace Services.SceneFlowServices
{
    public class SceneFlowService
    {
        
        public string CurrentScene { get; set; }
        
        private Dictionary<string, List<AsyncOperationHandle<GameObject>>> _sceneAssetHandles = new();


        public async UniTask SwitchScene(string sceneName, bool unloadCurrentScene = true, string [] assetKeys = null)
        {
            Debug.Log($"Loading new scene: {sceneName}", LogContext.SceneFlow);
            
            if (unloadCurrentScene && CurrentScene != GameConfig.BootScene)
            {
                await UnloadSceneAssets(CurrentScene);
                await SceneManager.UnloadSceneAsync(CurrentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            CurrentScene = sceneName;

            if (assetKeys == null) return;
            
            await LoadAddressables(assetKeys);
        }

        private async UniTask UnloadSceneAssets(string currentScene)
        {
            if (_sceneAssetHandles.TryGetValue(currentScene, out var handles))
            {
                foreach (var handle in handles)
                {
                    Debug.Log($"Releasing asset: {handle.DebugName}", LogContext.SceneFlow);
                    Addressables.Release(handle);
                    await UniTask.Yield();
                }
                _sceneAssetHandles.Remove(currentScene);
            }
        }

        public static void MoveObjectToScene(GameObject gameObject, string sceneName)
        {
            Debug.Log($"Moving {gameObject.name} to scene: {sceneName}", LogContext.SceneFlow);
            SceneManager.MoveGameObjectToScene(gameObject,
                SceneManager.GetSceneByName(sceneName));
        }

        private async UniTask LoadAddressables(IEnumerable<string> assetKeys)
        {
            if (!_sceneAssetHandles.ContainsKey(CurrentScene))
            {
                _sceneAssetHandles[CurrentScene] = new List<AsyncOperationHandle<GameObject>>();
            }
            
            foreach (var key in assetKeys)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(key);
                await handle.ToUniTask();
                var instance = await Addressables.InstantiateAsync(key).Task;
                MoveObjectToScene(instance, CurrentScene);
                _sceneAssetHandles[CurrentScene].Add(handle);
            }
        }
    }
}