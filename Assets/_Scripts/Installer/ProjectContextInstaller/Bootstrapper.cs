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
        [Inject] private LoadingController _loadingController;
        [Inject] private SceneFlowService _sceneFlowService;

        private CompositeDisposable _disposable = new();

        public async void Initialize()
        {
            _sceneFlowService.CurrentScene = GameConfig.SessionLoaderScene;
            await _loadingController.Appear();
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
