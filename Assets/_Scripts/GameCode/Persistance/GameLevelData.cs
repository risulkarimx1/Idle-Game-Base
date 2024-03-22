using System.Collections.Generic;
using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistance
{
    [DataIdentifier("game_level_data")]
    public class GameLevelData : BaseData
    {
        // serialization attribute for newtonsoft json
        [JsonProperty("mine_shaft_levels")] private Dictionary<int, int> _mineShaftLevels = new();
        
        public Dictionary<int, int> GetMineShaftLevels()
        {
            if(_mineShaftLevels == null || _mineShaftLevels.Count == 0)
            {
                _mineShaftLevels = new Dictionary<int, int> { { 1, 1 } };
                SetDirty();
            }

            return _mineShaftLevels;
        }

        public void SetMineshaftLevel(int mineShaftNumber, int level)
        {
            if (_mineShaftLevels.ContainsKey(mineShaftNumber) == false)
            {
                _mineShaftLevels.Add(mineShaftNumber, level);
            }
            else
            {
                _mineShaftLevels[mineShaftNumber] = level;
            }

            SetDirty();
        }
    }
}