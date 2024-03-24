﻿using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using GameCode.Persistence;
using LevelLoaderScripts;
using Services.DataFramework;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Elevator
{
    public class ElevatorModel : IAreaModel
    {
        private const double BasePrice = 60;
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        private readonly GameSessionProvider _gameSessionProvider;
        private readonly IReactiveProperty<double> _upgradePrice;
        private readonly IReactiveProperty<int> _level;
        
        [Inject]
        public ElevatorModel(GameConfig config, FinanceModel financeModel, CompositeDisposable disposable, GameSessionProvider gameSessionProvider)
        {
            _config = config;
            _financeModel = financeModel;
            _gameSessionProvider = gameSessionProvider;

            var elevatorLevel = GetElevatorLevel();
            _level = new ReactiveProperty<int>(elevatorLevel);
            
            StashAmount = new ReactiveProperty<double>();
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, 1) * Mathf.Pow(config.ActorUpgradeSkillIncrement, _level.Value - 1);
            _upgradePrice = new ReactiveProperty<double>(BasePrice * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1));
            CanUpgrade = _financeModel.Money
                .Select(money => money >= _upgradePrice.Value)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
        }

        private int GetElevatorLevel()
        {
            var elevatorLevel = _gameSessionProvider.SessionMineData().ElevatorLevel;
            return elevatorLevel;
        }

        public double SkillMultiplier { get; set; }

        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }

        public IReactiveProperty<double> StashAmount { get; }
        public IReadOnlyReactiveProperty<double> UpgradePrice => _upgradePrice;
        public IReadOnlyReactiveProperty<int> Level => _level;

        public void Upgrade()
        {
            if (_financeModel.Money.Value < _upgradePrice.Value) return;

            SkillMultiplier *= _config.ActorUpgradeSkillIncrement;
            var upgradePrice = _upgradePrice.Value;
            _upgradePrice.Value *= _config.ActorUpgradePriceIncrement;
            _financeModel.DrawResource(upgradePrice);
            _level.Value++;
        }
        
        public double DrawResource(double amount)
        {
            var result = 0d;
            if (StashAmount.Value <= amount)
            {
                result = StashAmount.Value;
                StashAmount.Value = 0;
            }
            else
            {
                result = amount;
                StashAmount.Value -= amount;
            }

            return result;
        }
    }
}