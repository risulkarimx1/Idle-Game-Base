using System;
using GameCode.Mineshaft;
using GameCode.Persistence;
using GameCode.Utils;
using LevelLoaderScripts;
using Services.DataFramework;
using Services.GameInitFramework;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Init
{
    public class GameInitializer : IInitializableAfterAll, IDisposable
    {
        [Inject(Id = GameConstants.FirtMinePositionObjectTag)] private Transform _mineshaftStartingPosition;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private IMineshaftFactory _mineshaftFactory;
        [Inject] private DataManager _dataManager;
        [Inject] private GameConfig _config;
        [Inject] private GameSessionProvider _gameSessionProvider;

        public void OnAllInitFinished()
        {
            var mineId = _gameSessionProvider.SessionMineId;
            var minesData = _dataManager.Get<MinesData>();
            
            var mineShaftLevels = minesData.ReadMineshaftLevels(mineId);
            var mineShaftPosition = _mineshaftStartingPosition.position;
            
            foreach (var mineShaftLevel in mineShaftLevels)
            {
               var controller = _mineshaftFactory.CreateMineshaft(mineId, mineShaftLevel.Key, mineShaftLevel.Value, mineShaftPosition);
               mineShaftPosition = controller.View.NextShaftView.NextShaftPosition;
            }
        }

        private string GetCurrentMineId(PlayerData playerData)
        {
            var mineId = string.Empty;
            if (!string.IsNullOrEmpty(playerData.MineId)) return mineId;
            
            mineId = _config.MinesConfig.DefaultMineInformation.MineId;
            playerData.MineId = mineId;

            return mineId;
        }

        public async void Dispose()
        {
            await _dataManager.SaveAllAsync();
            _disposable?.Dispose();
        }
    }
}