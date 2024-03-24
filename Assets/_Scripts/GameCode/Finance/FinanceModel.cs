using LevelLoaderScripts;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Finance
{
    public interface IFinanceModel
    {
        IReadOnlyReactiveProperty<double> Money { get; }
        IReactiveProperty<double> EarnedMoney { get; }
        void AddResource(double amount);
        double DrawResource(double amount);
    }
    public class FinanceModel: IFinanceModel
    {
        private readonly IReactiveProperty<double> _money;
        public IReadOnlyReactiveProperty<double> Money => _money;

        public IReactiveProperty<double> EarnedMoney => _earnedMoney;

        private readonly IReactiveProperty<double> _earnedMoney;
        [Inject] private GameSessionProvider _gameSessionProvider;
        
        public FinanceModel()
        {
            _money = new ReactiveProperty<double>(0);
            _earnedMoney = new ReactiveProperty<double>(_money.Value);
        }

        public void AddResource(double amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Received negative amount to add to inventory!");
                return;
            }

            _money.Value += amount;
            // TODO: We only add revenue from idle income. We can segregate that from different sources of income.
            _earnedMoney.Value += amount;
        }

        public double DrawResource(double amount)
        {
            var result = 0d;
            if (_money.Value <= amount)
            {
                result = _money.Value;
                _money.Value = 0;
            }
            else
            {
                result = amount;
                _money.Value -= amount;
            }

            return result;
        }
    }
}