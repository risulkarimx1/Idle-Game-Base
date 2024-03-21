using Services.LoadingScreen;
using Services.LogFramework;
using Services.SceneFlowServices;
using Zenject;

namespace LevelLoaderScripts
{
    public class GameLevelLoadingService: IInitializable
    {
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private LevelsConfig _levelsConfig;
        [Inject] private LoadingController _loadingController;

        public async void Initialize()
        {
            Debug.Log("All components loaded", LogContext.SceneFlow);
            var levelsAssets = _levelsConfig.GetLevelInformation("level_1");
            await _loadingController.Appear();
            await _sceneFlowService.SwitchScene(SceneFlowService.GameScene, true, levelsAssets);
            await _loadingController.Hide();
        }
    }
}