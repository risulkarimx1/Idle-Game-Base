using GameCode.Init;
using LevelLoaderScripts;
using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.LogFramework;
using Services.SceneFlowServices;
using Zenject;

namespace GameSessionLoaderScripts
{
    public class GameSessionLoader: IInitializableAfterAll
    {
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private LoadingController _loadingController;
        [Inject] private IGameSessionProvider _gameSessionProvider;
        
        public async void OnAllInitFinished()
        {
            Debug.Log($"All components loaded at {nameof(GameSessionLoader)}", LogContext.SceneFlow);
            var levelsAssets = _gameSessionProvider.GetSession().AssetsKey;
            await _loadingController.Appear();
            await _sceneFlowService.SwitchScene(GameConfig.GetInstance().GameScene, true, levelsAssets);
            await _loadingController.Hide();
        }
    }
}