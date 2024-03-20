using Services.LogFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = Services.LogFramework.Debug;

namespace Services.SceneFlowServices
{
    public class SceneFlowService
    {
        public static string GameScene { get; set; } = "GameScene";
        public static string BootScene { get; set; } = "BootScene";
        
        public string CurrentScene { get; set; }

        public void SwitchScene(string sceneName, bool unloadCurrentScene)
        {
            if (unloadCurrentScene && CurrentScene != BootScene)
            {
                Debug.Log($"Unloading current scene: {CurrentScene}", LogContext.SceneFlow);
                SceneManager.UnloadSceneAsync(CurrentScene);
            }
            
            Debug.Log($"Loading new scene: {sceneName}", LogContext.SceneFlow);
            if(CurrentScene == BootScene)
                SceneManager.LoadSceneAsync(sceneName);
            else
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            CurrentScene = sceneName;
        }
        
        public static void MoveObjectToScene(GameObject gameObject, string sceneName)
        {
            Debug.Log($"Moving object to scene: {sceneName}", LogContext.SceneFlow);
            SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
        }
    }
}