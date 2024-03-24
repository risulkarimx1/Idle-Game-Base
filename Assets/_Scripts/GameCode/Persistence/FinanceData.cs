using System.Collections.Generic;
using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [DataIdentifier("finance_data")]
    public class FinanceData: BaseData
    {
        [JsonProperty("deposit_rate")]
        private Dictionary<string,double> _depositRates = new();

        public void SetDepositRate(string mineId, double depositRate)
        {
            if (_depositRates.ContainsKey(mineId) == false)
            {
                _depositRates.Add(mineId, depositRate);
            }
            else
            {
                _depositRates[mineId] = depositRate;
            }
            SetDirty();
        }

        public double GetDepositRate(string mineId)
        {
            _depositRates.TryAdd(mineId, 0);

            return _depositRates[mineId];
        }
    }
}