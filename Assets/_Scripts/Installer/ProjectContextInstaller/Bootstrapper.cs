using GameCode.Init;
using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.SceneFlowServices;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    public class Bootstrapper: IInitializable, IInitializableAfterAll
    {
        [Inject] private LoadingController _loadingController;
        [Inject] private SceneFlowService _sceneFlowService;
        public async void Initialize()
        {
            _sceneFlowService.CurrentScene = GameConfig.GetInstance().SessionLoaderScene;
            await _loadingController.Appear();
        }

        public async void OnAllInitFinished()
        {
            await _loadingController.Hide();
        }
    }
}
