using System.Threading;
using Cysharp.Threading.Tasks;
using GameCode.Init;
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
        private CancellationTokenSource _cts;

        public async UniTask ShowAsync()
        {
            await PrepareItemsAsync();
            await _mineSelectionView.ShowMineSelectionUiFlowAsync();
            await ConfigureBackButtons();
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
                var mineInfoItemView = GameObject.Instantiate(_mineSelectionView.MineInfoItemViewPrefab,
                    _mineSelectionView.ContentParent);
                var isCurrentMine = _sessionProvider.GetSession().MineId == mine.Key;
                mineInfoItemView.SetMineInfo(mine.Key, mine.Value.MineName, mine.Value.MineDescription, OnMineSelected,
                    isCurrentMine);
            }

            await UniTask.Yield();
        }

        private async void OnMineSelected(string mineId)
        {
            if (_sessionProvider.GetSession().MineId == mineId) return;
            await _gameSessionUpdater.UpdateSession(mineId);
            await _mineSelectionView.HideMineSelectionUiFlow();
            Debug.Log($"Selected mine with id {mineId}", LogContext.LevelConfig);
            _cts?.Cancel();
            await _sceneFlowService.SwitchScene(GameConfig.SessionLoaderScene, true);
        }

        private async UniTask ConfigureBackButtons()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var backButtonTask = _mineSelectionView.BackdropButton.OnClickAsObservable().First().ToUniTask(cancellationToken: _cts.Token);
            var closeButtonTask = _mineSelectionView.CloseButton.OnClickAsObservable().First().ToUniTask(cancellationToken: _cts.Token);
            await UniTask.WhenAny(new[] { backButtonTask, closeButtonTask });
            await _mineSelectionView.HideMineSelectionUiFlow();
        }
    }
}