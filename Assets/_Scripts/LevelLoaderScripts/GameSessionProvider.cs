using Cysharp.Threading.Tasks;
using GameCode.Init;
using GameCode.Persistence;
using Services.DataFramework;
using Services.GameInitFramework;
using Zenject;

namespace LevelLoaderScripts
{
    public class GameSessionProvider
    {
        [Inject] private DataManager _dataManager;
        [Inject] private GameConfig _config;

        public string SessionMineId
        {
            get
            {
                var value = _dataManager.Get<PlayerData>().MineId;
                if (string.IsNullOrEmpty(value))
                {
                    value = _config.MinesConfig.DefaultMineInformation.MineId;
                }

                return value;
            }
            private set
            {
                _dataManager.Get<PlayerData>().MineId = value;
            }
        }

        public string[] AssetsKey => _config.MinesConfig.GetLevelInformation(SessionMineId).AssetKeys;
        
        public MineData MineData()
        {
            return _dataManager.Get<MinesData>().GetMineData(SessionMineId);
        }

        public async UniTask UpdateMineId(string mineId)
        {
            _dataManager.Get<PlayerData>().MineId = mineId;
            await _dataManager.SaveAllAsync();
        }
    }
}