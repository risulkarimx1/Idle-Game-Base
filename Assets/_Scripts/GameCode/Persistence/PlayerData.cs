using Newtonsoft.Json;
using Services.DataFramework;

namespace GameCode.Persistence
{
    [DataIdentifier("payer_data")]
    public class PlayerData: BaseData
    {
        [JsonProperty("player_cash")] private int _playerCash;
        [JsonProperty("selected_mine")] private string _mineId;

        public int PlayerCash
        {
            get => _playerCash;
            set
            {
                _playerCash = value;
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