using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using LevelLoaderScripts;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Warehouse
{
    public class WarehouseModel : IAreaModel
    {
        private readonly GameConfig _config;
        private readonly IFinanceModel _financeModel;
        
        private readonly IReactiveProperty<double> _upgradePrice;
        private readonly IReactiveProperty<int> _level;

        [Inject]
        public WarehouseModel( GameConfig config, IFinanceModel financeModel, CompositeDisposable disposable, IGameSessionProvider gameSessionProvider)
        {
            _config = config;
            _financeModel = financeModel;

            var warehouseLevel = gameSessionProvider.GetSession().WarehouseLevel;
            _level = new ReactiveProperty<int>(warehouseLevel);
            
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, 1) * Mathf.Pow(config.ActorUpgradeSkillIncrement, _level.Value - 1);
            _upgradePrice = new ReactiveProperty<double>(_config.WareHouseBasePrice * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1));
            CanUpgrade = _financeModel.Money
                .Select(money => money >= _upgradePrice.Value)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);

        }

        public double SkillMultiplier { get; set; }

        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }

        public void AddResource(double amount) => _financeModel.AddResource(amount);
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
    }
}