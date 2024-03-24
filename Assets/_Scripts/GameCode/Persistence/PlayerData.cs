using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [DataIdentifier("payer_data")]
    public class PlayerData: BaseData
    {
        [JsonProperty("player_money")] private double _money;
        [JsonProperty("selected_mine")] private string _mineId;

        public double Money
        {
            get => _money;
            set
            {
                _money = value;
                SetDirty();
            }
        }

        public string MineId
        {
            get => _mineId;
            set
            {
                _mineId = value;
                SetDirty();
            }
        }
    }
}