using System.Collections.Generic;
using Newtonsoft.Json;
using Services.DataFramework;
using Services.Utils;

namespace GameCode.Persistence
{
    [DataIdentifier("mines_data")]
    public class MinesData : BaseData
    {
        [JsonProperty("mine_data")] private Dictionary<string, MineData> _mineData = new();
        
        public MineData GetMineData(string mineId)
        {
            TryCreateMineData(mineId);
            return _mineData[mineId];
        }

        private Dictionary<int, int> GetMineshaftLevels(string mineId)
        {
            var mineData = GetMineData(mineId);
            return mineData.GetMineShaftsLevel();
        }

        public Dictionary<int, int> ReadMineshaftLevels(string mineId)
        {
            return GetMineshaftLevels(mineId).Clone();
        }

        private void TryCreateMineData(string mineId)
        {
            if (!_mineData.TryGetValue(mineId, out _))
            {
                _mineData[mineId] = new MineData(mineId);
            }

            _mineData[mineId].Initialize(SetDirty);
        }
    }
}