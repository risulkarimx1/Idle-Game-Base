using System;
using GameCode.Finance;
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
        [Inject] private FinanceModel _financeModel;

        public void OnAllInitFinished()
        {
            var mineId = _gameSessionProvider.SessionMineId;
            var minesData = _dataManager.Get<MinesData>();
            
            SetupMineShafts(minesData, mineId);

            SetupFinanceModel();
        }

        private void SetupFinanceModel()
        {
            var money = Math.Max(_config.StartingMoney, _dataManager.Get<PlayerData>().Money);
            _financeModel.AddResource(money);
            _financeModel.Money.Subscribe(value =>
            {
                _dataManager.Get<PlayerData>().Money = value;
            }).AddTo(_disposable);
        }

        private void SetupMineShafts(MinesData minesData, string mineId)
        {
            var mineShaftLevels = minesData.ReadMineshaftLevels(mineId);
            var mineShaftPosition = _mineshaftStartingPosition.position;
            
            foreach (var mineShaftLevel in mineShaftLevels)
            {
                var controller = _mineshaftFactory.CreateMineshaft(mineId, mineShaftLevel.Key, mineShaftLevel.Value, mineShaftPosition);
                mineShaftPosition = controller.View.NextShaftView.NextShaftPosition;
            }
        }

        public async void Dispose()
        {
            await _dataManager.SaveAllAsync();
            _disposable?.Dispose();
        }
    }
}