using Cysharp.Threading.Tasks;
using GameCode.Init;
using GameCode.Utils;
using LevelLoaderScripts;
using Services.LogFramework;
using Services.SceneFlowServices;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Debug = Services.LogFramework.Debug;

namespace GameCode.Mines
{
    public class MineSelectionController
    {
        [Inject] private MineSelectionView _mineSelectionView;
        [Inject] private GameConfig _config;
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private IGameSessionProvider _sessionProvider;
        [Inject] private IGameSessionUpdater _gameSessionUpdater;
        [Inject] private CompositeDisposable _disposable;
        
        public async UniTask ShowAsync()
        {
            await PrepareItemsAsync();
            await _mineSelectionView.ShowMineSelectionUiFlowAsync();
            ConfigureBackButtons();
        }

        private async UniTask PrepareItemsAsync()
        {
            // destroy all children of _mineSelectionView.ContentParent
            foreach (Transform child in _mineSelectionView.ContentParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            await UniTask.Yield();
            foreach (var mine in _config.MinesConfig.MinesInformation)
            {
                var mineInfoItemView = GameObject.Instantiate(_mineSelectionView.MineInfoItemViewPrefab, _mineSelectionView.ContentParent);
                var isCurrentMine = _sessionProvider.GetSession().MineId == mine.Key;
                mineInfoItemView.SetMineInfo(mine.Key, mine.Value.MineName, mine.Value.MineDescription, OnMineSelected, isCurrentMine);
            }

            await UniTask.Yield();
        }

        private async void OnMineSelected(string mineId)
        {
            if(_sessionProvider.GetSession().MineId == mineId) return;
            await _gameSessionUpdater.UpdateSession(mineId);
            await _mineSelectionView.HideMineSelectionUiFlow();
            Debug.Log($"Selected mine with id {mineId}", LogContext.LevelConfig);
            await _sceneFlowService.SwitchScene(GameConfig.SessionLoaderScene, true);
        }
        
        private void ConfigureBackButtons()
        {
            ConfigureButton(_mineSelectionView.BackdropButton);
            ConfigureButton(_mineSelectionView.CloseButton);
        }

        private void ConfigureButton(Button button)
        {
            button.OnClickAsObservable().Subscribe(async _ =>
            {
                await _mineSelectionView.HideMineSelectionUiFlow();
            }).AddTo(_disposable);
        }
    }
}