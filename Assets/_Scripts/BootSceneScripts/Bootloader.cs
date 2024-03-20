using System;
using Frameworks.GameInitFramework;
using Frameworks.LoadingScreen;
using GameCode.Init;
using Services.SceneFlowServices;
using UniRx;
using Zenject;

namespace BootSceneScripts
{
    public class Bootloader: IInitializable, IDisposable, IInitializableAfterAll
    {
        [Inject] private IInitProgressReporter _progressReporter;
        [Inject] private LoadingController _loadingController;
        [Inject] private SceneFlowService _sceneFlowService;

        private CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _sceneFlowService.CurrentScene = SceneFlowService.BootScene;
            _progressReporter.OnProgressUpdated.Subscribe(async value =>
            {
                await _loadingController.Update(value);
            }).AddTo(_disposable);
        }
        
        // add a method to hot reload the game
        public void SoftReloadGame()
        {
            _sceneFlowService.SwitchScene(SceneFlowService.BootScene, true);
        }

        public void OnAllInitFinished()
        {
            _sceneFlowService.SwitchScene(SceneFlowService.GameScene, true);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
