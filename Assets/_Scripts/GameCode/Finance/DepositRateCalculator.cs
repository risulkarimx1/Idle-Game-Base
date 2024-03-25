using System;
using System.Collections.Generic;
using System.Linq;
using GameCode.Persistence;
using GameCode.Signals;
using GameCode.TimeProvider;
using LevelLoaderScripts;
using Services.DataFramework;
using Services.GameInitFramework;
using Services.LogFramework;
using UniRx;
using Zenject;

namespace GameCode.Finance
{
    public class DepositRateCalculator : IInitializableAfterAll
    {
        [Inject] private readonly CompositeDisposable _disposables;
        [Inject] private ITimeProvider _timeProvider;
        [Inject] private IDataManager _dataManager;
        [Inject] private IGameSessionProvider _sessionProvider;
        [Inject] private SignalBus _signalBus;

        private readonly List<(DateTime Time, double Amount)> _deposits = new();
        private const int SampleSize = 3;
        
        public void OnAllInitFinished()
        {
            var financeData = _dataManager.Get<FinanceData>();
            var mineId = _sessionProvider.GetSession().MineId;
            _signalBus.GetStream<GameSignals.DepositSignal>().Subscribe(s =>
            {
                var now = _timeProvider.UtcNow;
                _deposits.Add((now, s.Amount));
                    
                if (_deposits.Count < SampleSize) return;
                    
                var currentIncomeRate = financeData.GetDepositRate(mineId);
                var newIncomeRate = CalculateIncomeRate();
                var incomeRate = Math.Max(currentIncomeRate, newIncomeRate);
                Debug.Log($"{mineId}: NewIncomeRate {newIncomeRate}. Current Income Rate: {currentIncomeRate}. Income rate updated to {incomeRate}", LogContext.FinanceModel);
                    
                financeData.SetDepositRate(mineId, incomeRate);
                _deposits.Clear();
            }).AddTo(_disposables);
        }

        private double CalculateIncomeRate()
        {
            var totalIncome = _deposits.Sum(d => d.Amount);
            var firstIncomeTime = _deposits[0].Time;
            var lastIncomeTime = _deposits[SampleSize - 1].Time;
            var timeTakenInSeconds = (lastIncomeTime - firstIncomeTime).TotalSeconds;
            var incomeRate = totalIncome / timeTakenInSeconds;
            return incomeRate;
        }
    }
}
