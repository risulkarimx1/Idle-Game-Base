using System;
using GameCode.Init;
using Services.GameInitFramework;
using Services.LoadingScreen;
using Services.SceneFlowServices;
using UniRx;
using Zenject;

namespace Installer.ProjectContextInstaller
{
    public class Bootstrapper: IInitializable, IDisposable, IInitializableAfterAll
    {
        [Inject] private IInitProgressReporter _progressReporter;
        [Inject] private LoadingController _loadingController;
        [Inject] private SceneFlowService _sceneFlowService;

        private CompositeDisposable _disposable = new();

        public async void Initialize()
        {
            _sceneFlowService.CurrentScene = SceneFlowService.LevelLoaderScene;
            await _loadingController.Appear();
            _progressReporter.OnProgressUpdated.Subscribe(async value =>
            {
                
            }).AddTo(_disposable);
        }
        
        // add a method to hot reload the game
        public async void SoftReloadGame()
        {
            await _sceneFlowService.SwitchScene(SceneFlowService.LevelLoaderScene, true);
        }

        public async void OnAllInitFinished()
        {
            await _loadingController.Hide();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
