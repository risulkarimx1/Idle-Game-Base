using System;
using System.Collections.Generic;
using GameCode.Utils;
using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [Serializable]
    public class MineData
    {
        [JsonProperty("mine_id")] 
        private string _mineId;

        private Action _dataUpdated;

        [JsonProperty("mine_shaft_levels")] 
        private Dictionary<int, int> _mineShaftLevels;
        [JsonProperty("elevator_level")]
        private int _elevatorLevel;
        [JsonProperty("warehouse_level")]
        private int _warehouseLevel;

        public MineData(string mineId)
        {
            _mineId = mineId;
            _mineShaftLevels = new Dictionary<int, int>
            {
                { 1, 1 }
            };
        }

        public void Initialize(Action dataUpdated)
        {
            _dataUpdated = dataUpdated;
        }

        public int ElevatorLevel
        {
            get => Math.Max(1, _elevatorLevel);
            set
            {
                _elevatorLevel = value;
                _dataUpdated?.Invoke();
            }
        }

        public int WarehouseLevel
        {
            get => Math.Max(1, _warehouseLevel);
            set
            {
                _warehouseLevel = value;
                _dataUpdated?.Invoke();
            }
        }
        
        public void Copy(MineData mineData)
        {
            _mineId = mineData._mineId;
            _mineShaftLevels = new Dictionary<int, int>(mineData._mineShaftLevels);
            _elevatorLevel = mineData._elevatorLevel;
            _warehouseLevel = mineData._warehouseLevel;
        }

        public Dictionary<int, int> GetMineShaftsLevel()
        {
            return _mineShaftLevels;
        }
        
        public int GetMineShaftLevel(int mineShaftNumber)
        {
            TryCreateMineshaft(mineShaftNumber);
            return _mineShaftLevels[mineShaftNumber];
        }
        public void SetMineShaftLevel(int mineShaftNumber, int level)
        {
            TryCreateMineshaft(mineShaftNumber);
            _mineShaftLevels[mineShaftNumber] = level;
            _dataUpdated?.Invoke();
        }

        private void TryCreateMineshaft(int mineShaftNumber)
        {
            _mineShaftLevels.TryAdd(mineShaftNumber, 1);
        }
    }
    
    [DataIdentifier("mines_data")]
    public class MinesData : BaseData
    {
        [JsonProperty("mine_data")] 
        private Dictionary<string, MineData> _mineData = new();

        // Get Methods
        public MineData GetMineData(string mineId)
        {
            TryCreateMineData(mineId);
            return _mineData[mineId];
        }

        public Dictionary<int, int> GetMineshaftLevels(string mineId)
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