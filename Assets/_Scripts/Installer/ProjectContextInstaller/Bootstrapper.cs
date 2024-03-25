using GameCode.Init;
using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.SceneFlowServices;
using UnityEngine.SceneManagement;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    public class Bootstrapper : IInitializable, IInitializableAfterAll
    {
        [Inject] private LoadingController _loadingController;
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private GameConfig _config;

        public async void Initialize()
        {
            if (SceneManager.GetActiveScene().name != GameConfig.GetInstance().BootScene)
            {
                SceneManager.LoadScene(_config.BootScene);
            }
            else
            {
                _sceneFlowService.CurrentScene = GameConfig.GetInstance().SessionLoaderScene;
                await _loadingController.Appear();    
            }
        }

        public async void OnAllInitFinished()
        {
            await _loadingController.Hide();
        }
    }
}