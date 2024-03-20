using Frameworks.LogFramework;

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
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CurrentScene);
            }
            Debug.Log($"Loading new scene: {sceneName}", LogContext.SceneFlow);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            CurrentScene = sceneName;
        }
    }
}