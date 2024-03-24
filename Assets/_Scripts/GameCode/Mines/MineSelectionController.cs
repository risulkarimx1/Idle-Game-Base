using Cysharp.Threading.Tasks;
using GameCode.Init;
using GameCode.Persistence;
using LevelLoaderScripts;
using Services.DataFramework;
using Services.SceneFlowServices;
using UnityEditor.SearchService;
using UnityEngine;
using Zenject;

namespace GameCode.Mines
{
    public class MineSelectionController
    {
        [Inject] private MineSelectionView _mineSelectionView;
        [Inject] private GameConfig _config;
        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private GameSessionProvider _sessionProvider;
        
        public async UniTask ShowAsync()
        {
            await PrepareItemsAsync();
            await _mineSelectionView.ShowMineSelectionUiFlowAsync();
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
                mineInfoItemView.SetMineInfo(mine.Key, mine.Value.MineName, mine.Value.MineDescription, OnMineSelected);
            }

            await UniTask.Yield();
        }

        private async void OnMineSelected(string mineId)
        {
            await _sessionProvider.UpdateSessionMineId(mineId);
            await _mineSelectionView.HideMineSelectionUiFlow();
            Debug.Log($"Selected mine with id {mineId}");
            await _sceneFlowService.SwitchScene(SceneFlowService.LevelLoaderScene, true);
        }
    }
}