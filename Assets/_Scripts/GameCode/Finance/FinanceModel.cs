using GameCode.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Finance
{
    public interface IFinanceModel
    {
        IReadOnlyReactiveProperty<double> Money { get; }
        void AddResource(double amount, bool initialDeposit = false);
        double DrawResource(double amount);
    }

    public class FinanceModel : IFinanceModel
    {
        private readonly IReactiveProperty<double> _money;
        public IReadOnlyReactiveProperty<double> Money => _money;
        [Inject] private SignalBus _signalBus;

        public FinanceModel()
        {
            _money = new ReactiveProperty<double>(0);
        }

        public void AddResource(double amount, bool initialDeposit = false)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Received negative amount to add to inventory!");
                return;
            }

            _money.Value += amount;

            if (!initialDeposit)
                _signalBus.Fire(new GameSignals.DepositSignal(amount));
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