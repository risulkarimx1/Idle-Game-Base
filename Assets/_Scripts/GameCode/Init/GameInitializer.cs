using System;
using GameCode.Finance;
using GameCode.Mineshaft;
using GameCode.Persistence;
using GameCode.TimeProvider;
using GameCode.UI;
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
        [Inject(Id = GameConstants.FirtMinePositionObjectTag)]
        private Transform _mineshaftStartingPosition;

        [Inject] private CompositeDisposable _disposable;
        [Inject] private IMineshaftFactory _mineshaftFactory;
        [Inject] private IDataManager _dataManager;
        [Inject] private GameConfig _config;
        [Inject] private IGameSessionProvider _gameSessionProvider;
        [Inject] private FinanceModel _financeModel;
        [Inject] private ITimeProvider _timeProvider;
        [Inject] private HudController _hudController;

        public void OnAllInitFinished()
        {
            SetupMineShafts();
            SetupFinanceModel();
            SetupPassiveIncome();
        }

        private void SetupMineShafts()
        {
            var mineId = _gameSessionProvider.GetSession().MineId;
            var minesData = _dataManager.Get<MinesData>();
            var mineShaftLevels = minesData.ReadMineshaftLevels(mineId);
            var mineShaftPosition = _mineshaftStartingPosition.position;

            foreach (var mineShaftLevel in mineShaftLevels)
            {
                var controller = _mineshaftFactory.CreateMineshaft(mineId, mineShaftLevel.Key, mineShaftLevel.Value,
                    mineShaftPosition);
                mineShaftPosition = controller.View.NextShaftView.NextShaftPosition;
            }
        }

        private void SetupFinanceModel()
        {
            var money = _dataManager.Get<FinanceData>().Money;
            _financeModel.AddResource(money, true);
            _financeModel.Money.Subscribe(value => { _dataManager.Get<FinanceData>().Money = value; })
                .AddTo(_disposable);
        }

        private void SetupPassiveIncome()
        {
            if (_config.EnablePassiveIncome == false) return;
            var mineId = _gameSessionProvider.GetSession().MineId;
            var incomeRate = _dataManager.Get<FinanceData>().GetDepositRate(mineId);
            var logOffTime = _dataManager.Get<GameSessionData>().GetLogOffTime(mineId, _timeProvider);
            var timeDifferenceInSecond = (_timeProvider.UtcNow - logOffTime).TotalSeconds;
            var totalPassiveIncome = incomeRate * timeDifferenceInSecond;
            if (totalPassiveIncome <= 0) return;
            _financeModel.AddResource(totalPassiveIncome, true);
            _hudController.ShowPassiveIncomeTooltip($"+{(int)totalPassiveIncome}");
        }


        public async void Dispose()
        {
            await _dataManager.SaveAllAsync();
            _disposable?.Dispose();
        }
    }
}