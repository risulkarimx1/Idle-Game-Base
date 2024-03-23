using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.LogFramework;
using Services.SceneFlowServices;
using Zenject;

namespace LevelLoaderScripts
{
    public class GameLevelLoadingService: IInitializableAfterAll
    {
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private LoadingController _loadingController;
        [Inject] private GameSessionProvider _gameSessionProvider;
        
        public async void OnAllInitFinished()
        {
            Debug.Log("All components loaded", LogContext.SceneFlow);
            var levelsAssets = _gameSessionProvider.AssetsKey;
            await _loadingController.Appear();
            await _sceneFlowService.SwitchScene(SceneFlowService.GameScene, true, levelsAssets);
            await _loadingController.Hide();
        }
    }
}