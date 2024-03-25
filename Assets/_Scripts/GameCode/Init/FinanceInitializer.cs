using GameCode.Finance;
using GameCode.Persistence;
using GameCode.TimeProvider;
using GameCode.UI;
using GameSessions;
using Services.DataFramework;
using Services.GameInitFramework;
using UniRx;
using Zenject;

namespace GameCode.Init
{
    public class FinanceInitializer: IInitializableAfterAll
    {
        [Inject] private CompositeDisposable _disposable;
        [Inject] private IDataManager _dataManager;
        [Inject] private GameConfig _config;
        [Inject] private IGameSessionProvider _gameSessionProvider;
        [Inject] private FinanceModel _financeModel;
        [Inject] private ITimeProvider _timeProvider;
        [Inject] private HudController _hudController;
        
        public void OnAllInitFinished()
        {
            SetupFinanceModel();
            SetupPassiveIncome();
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
            
            if (totalPassiveIncome > _config.MaximumPassiveIncome)
            {
                totalPassiveIncome = _config.MaximumPassiveIncome;
            }
            _financeModel.AddResource(totalPassiveIncome, true);
            _hudController.ShowPassiveIncomeTooltip($"+{(int)totalPassiveIncome}");
        }
    }
}