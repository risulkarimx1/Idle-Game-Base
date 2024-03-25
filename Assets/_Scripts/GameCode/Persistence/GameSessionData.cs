using System;
using System.Collections.Generic;
using GameCode.TimeProvider;
using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [DataIdentifier("game_session_data")]
    public class GameSessionData: BaseData
    {
        [JsonProperty("session_logoff_time")]
        private Dictionary<string, DateTime> _sessionLogOffTimes = new();

        [JsonProperty("session_mine")] private string _mineId;

        public string MineId
        {
            get => _mineId;
            set
            {
                _mineId = value;
                SetDirty();
            }
        }
        
        public void UpdateLoggOffTime(string mineId, ITimeProvider timeProvider)
        {
            if (_sessionLogOffTimes.ContainsKey(mineId) == false)
                _sessionLogOffTimes.Add(mineId, timeProvider.UtcNow);
            else
                _sessionLogOffTimes[mineId] = timeProvider.UtcNow;
            SetDirty();
        }
        
        public DateTime GetLogOffTime(string mineId, ITimeProvider timeProvider)
        {
            if (_sessionLogOffTimes.ContainsKey(mineId) == false)
            {
                _sessionLogOffTimes.Add(mineId, timeProvider.UtcNow);
            }

            return _sessionLogOffTimes[mineId];
        }
    }
}