using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Services.LoadingScreen;
using Services.LogFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;
using Debug = Services.LogFramework.Debug;

namespace Services.SceneFlowServices
{
    public class SceneFlowService
    {
        public static string GameScene { get; set; } = "GameScene";
        
        public static string LevelLoaderScene { get; set; } = "LevelLoaderScene";
        public static string BootScene { get; set; } = "BootScene";

        public string CurrentScene { get; set; }

        public async UniTask SwitchScene(string sceneName, bool unloadCurrentScene = true, string [] assetKeys = null)
        {
            Debug.Log($"Loading new scene: {sceneName}", LogContext.SceneFlow);
            
            if (unloadCurrentScene && CurrentScene != BootScene)
            {
                await SceneManager.UnloadSceneAsync(CurrentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            CurrentScene = sceneName;

            if (assetKeys == null) return;
            
            await LoadAddressables(assetKeys);
            foreach (var key in assetKeys)
            {
                var handle = Addressables.InstantiateAsync(key);
                while (!handle.IsDone)
                {
                    // Update loading screen with the progress
                    // await _loadingController.Update(handle.PercentComplete);
                    await UniTask.Yield();
                }
                
                MoveObjectToScene(handle.Result, CurrentScene);
            }
        }

        public static void MoveObjectToScene(GameObject gameObject, string sceneName)
        {
            Debug.Log($"Moving object to scene: {sceneName}", LogContext.SceneFlow);
            SceneManager.MoveGameObjectToScene(gameObject,
                SceneManager.GetSceneByName(sceneName));
        }

        private async UniTask LoadAddressables(IEnumerable<string> assetKeys)
        {
            var loadTasks =
                Enumerable.Select(
                    Enumerable.Select(assetKeys.Select(Addressables.LoadAssetAsync<GameObject>),
                        handle => handle.ToUniTask()),
                    t => (UniTask)t).ToList();

            await UniTask.WhenAll(loadTasks);
        }
    }
}