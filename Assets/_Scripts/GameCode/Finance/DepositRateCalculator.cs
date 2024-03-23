using System;
using System.Collections.Generic;
using GameCode.Persistence;
using GameCode.TimeProvider;
using Services.DataFramework;
using Services.GameInitFramework;
using Services.LogFramework;
using UniRx;
using Zenject;

namespace GameCode.Finance
{
    public class DepositRateCalculator : IInitializableAfterAll
    {
        [Inject] private IFinanceModel _financeModel;
        [Inject] private readonly CompositeDisposable _disposables;
        [Inject] private ITimeProvider _timeProvider;
        [Inject] private DataManager _dataManager;

        private readonly List<(DateTime Time, double Amount)> _deposits = new();
        private const int SampleSize = 3;
        
        public void OnAllInitFinished()
        {
            var financeData = _dataManager.Get<FinanceData>();
            
            _financeModel.EarnedMoney.Skip(1) // Skip initial value
                .Subscribe(newBalance =>
                {
                    var now = _timeProvider.UtcNow;

                    if (newBalance <= 0) return;
                    
                    _deposits.Add((now, newBalance));
                    
                    if (_deposits.Count < SampleSize) return;
                    
                    var currentIncomeRate = financeData.GetDepositRate(1);
                    var newIncomeRate = CalculateIncomeRate();
                    var incomeRate = Math.Max(currentIncomeRate, newIncomeRate);
                    Debug.Log($"NewIncomeRate {newIncomeRate}. Current Income Rate: {currentIncomeRate}. Income rate updated to {incomeRate}", LogContext.FinanceModel);
                    
                    financeData.SetDepositRate(1, incomeRate);
                    _deposits.Clear();

                }).AddTo(_disposables);
        }

        private double CalculateIncomeRate()
        {
            var firstIncome = _deposits[0].Amount;
            var lastIncome = _deposits[SampleSize - 1].Amount;
            var earning = lastIncome - firstIncome;

            var firstIncomeTime = _deposits[0].Time;
            var lastIncomeTime = _deposits[SampleSize - 1].Time;

            var timeGapInSeconds = (lastIncomeTime - firstIncomeTime).TotalSeconds;
            
            var incomeRate = earning / timeGapInSeconds;
            return incomeRate;
        }
    }
}
