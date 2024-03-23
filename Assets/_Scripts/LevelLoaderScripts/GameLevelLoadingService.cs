using GameCode.Init;
using GameCode.Mines;
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
        [Inject] private GameConfig _gameConfig;
        [Inject] private LoadingController _loadingController;
        

        public async void OnAllInitFinished()
        {
            Debug.Log("All components loaded", LogContext.SceneFlow);
            var levelsAssets = _gameConfig.MinesConfig.GetLevelInformation("mine_1").AssetKeys;
            await _loadingController.Appear();
            await _sceneFlowService.SwitchScene(SceneFlowService.GameScene, true, levelsAssets);
            await _loadingController.Hide();
        }
    }
}