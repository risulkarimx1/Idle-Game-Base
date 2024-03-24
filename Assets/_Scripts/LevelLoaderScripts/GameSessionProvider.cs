using Cysharp.Threading.Tasks;
using GameCode.Init;
using GameCode.Persistence;
using GameCode.TimeProvider;
using Services.DataFramework;
using Zenject;

namespace LevelLoaderScripts
{
    public class GameSessionProvider
    {
        [Inject] private DataManager _dataManager;
        [Inject] private GameConfig _config;
        [Inject] private ITimeProvider _timeProvider;

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
        
        public MineData SessionMineData()
        {
            return _dataManager.Get<MinesData>().GetMineData(SessionMineId);
        }

        public async UniTask UpdateSessionMineId(string mineId)
        {
            _dataManager.Get<PlayerData>().MineId = mineId;
            _dataManager.Get<PassiveIncomeData>().UpdateLoggOffTime(mineId, _timeProvider);
            await _dataManager.SaveAllAsync();
        }
    }
}