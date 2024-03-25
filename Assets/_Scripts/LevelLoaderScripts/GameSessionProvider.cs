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
                var value = _dataManager.Get<GameSessionData>().MineId;
                if (string.IsNullOrEmpty(value))
                {
                    value = _config.MinesConfig.DefaultMineInformation.MineId;
                }

                return value;
            }
        }

        public string[] AssetsKey => _config.MinesConfig.GetLevelInformation(SessionMineId).AssetKeys;
        
        public MineData SessionMineData()
        {
            return _dataManager.Get<MinesData>().GetMineData(SessionMineId);
        }

        public async UniTask UpdateSessionMineId(string mineId)
        {
            _dataManager.Get<GameSessionData>().MineId = mineId;
            _dataManager.Get<GameSessionData>().UpdateLoggOffTime(mineId, _timeProvider);
            await _dataManager.SaveAllAsync();
        }
    }
}