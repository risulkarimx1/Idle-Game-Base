using GameCode.Init;
using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.LogFramework;
using Services.SceneFlowServices;
using Zenject;

namespace BootSceneScripts
{
    
    public class BootSceneManager : IInitializableAfterAll
    {
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private LoadingController _loadingController;

        public async void OnAllInitFinished()
        {
            Debug.Log($"Game Initialized at {nameof(BootSceneManager)}", LogContext.SceneFlow);
            await _loadingController.Appear();
            await _sceneFlowService.SwitchScene(GameConfig.GetInstance().SessionLoaderScene, false);
            await _loadingController.Hide();
        }
    }
}