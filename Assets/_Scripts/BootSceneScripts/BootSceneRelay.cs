using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.LogFramework;
using Services.SceneFlowServices;
using Zenject;

namespace BootSceneScripts
{
    public class BootSceneRelay: IInitializable, IInitializableAfterAll
    {
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private LoadingController _loadingController;
        
        public async void Initialize()
        {
            await _loadingController.Appear();  
            await _sceneFlowService.SwitchScene(SceneFlowService.LevelLoaderScene, false);
            await _loadingController.Hide();
        }

        public async void OnAllInitFinished()
        {
            Debug.Log($"here in {nameof(BootSceneRelay)}", LogContext.SceneFlow);
            // await _loadingController.Update(0);    
            // await _sceneFlowService.SwitchScene(SceneFlowService.LevelLoaderScene, false);
            // await _loadingController.Update(1);
        }
    }
}