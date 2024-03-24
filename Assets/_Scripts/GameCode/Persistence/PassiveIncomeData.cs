using System;
using System.Collections.Generic;
using GameCode.TimeProvider;
using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [DataIdentifier("passive_income_data")]
    public class PassiveIncomeData: BaseData
    {
        [JsonProperty("mine_logoff_time")]
        public Dictionary<string, DateTime> _mineLogOffTimes = new();

        public void UpdateLoggOffTime(string mineId, ITimeProvider timeProvider)
        {
            if (_mineLogOffTimes.ContainsKey(mineId) == false)
                _mineLogOffTimes.Add(mineId, timeProvider.UtcNow);
            else
                _mineLogOffTimes[mineId] = timeProvider.UtcNow;
            SetDirty();
        }
        
        public DateTime GetLogOffTime(string mineId, ITimeProvider timeProvider)
        {
            if (_mineLogOffTimes.ContainsKey(mineId) == false)
            {
                _mineLogOffTimes.Add(mineId, timeProvider.UtcNow);
            }

            return _mineLogOffTimes[mineId];
        }
    }
}