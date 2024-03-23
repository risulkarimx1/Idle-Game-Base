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

        public int ElevatorLevel
        {
            get => _elevatorLevel;
            set => _elevatorLevel = value;
        }

        public int WarehouseLevel
        {
            get => _warehouseLevel;
            set => _warehouseLevel = value;
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

        public int GetElevatorLevel(string mineId)
        {
            var mineData = GetMineData(mineId);
            return mineData.ElevatorLevel;
        }
        public int GetWarehouseLevel(string mineId)
        {
            var mineData = GetMineData(mineId);
            return mineData.WarehouseLevel;
        }
        
        public void UpdateMineData(string mineId, MineData newMineData)
        {
            var mineData = GetMineData(mineId);
            mineData.Copy(newMineData);
            SetDirty();
        }

        private void TryCreateMineData(string mineId)
        {
            if (_mineData.ContainsKey(mineId)) return;
            var mineData = new MineData(mineId);
            _mineData.Add(mineId, mineData);
        }
        
        
        // Set Method
        
        public void SetMineShaftLevel(string mineId, int mineShaftNumber, int level)
        {
            var mineData = GetMineData(mineId);
            mineData.SetMineShaftLevel(mineShaftNumber, level);
            SetDirty();
        }

        public void SetWarehouseLevel(string mineId, int level)
        {
            var mineData = GetMineData(mineId);
            mineData.WarehouseLevel = level;
            SetDirty();
        }

        public void SetElevatorLevel(string mineId, int level)
        {
            var mineData = GetMineData(mineId);
            mineData.ElevatorLevel = level;
            SetDirty();
        }
    }
}