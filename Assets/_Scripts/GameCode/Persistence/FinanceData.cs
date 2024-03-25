using System.Collections.Generic;
using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [DataIdentifier("finance_data")]
    public class FinanceData : BaseData
    {
        [JsonProperty("deposit_rate")] private Dictionary<string, double> _depositRates = new();

        [JsonProperty("player_money")] private double _money;

        public double Money
        {
            get => _money;
            set
            {
                _money = value;
                SetDirty();
            }
        }

        public void SetDepositRate(string mineId, double depositRate)
        {
            _depositRates[mineId] = depositRate;
            SetDirty();
        }

        public double GetDepositRate(string mineId)
        {
            _depositRates.TryAdd(mineId, 0);

            return _depositRates[mineId];
        }
    }
}