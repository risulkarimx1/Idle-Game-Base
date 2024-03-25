using Cysharp.Threading.Tasks;
using GameCode.Init;
using GameCode.Persistence;
using GameCode.TimeProvider;
using Services.DataFramework;
using Zenject;

namespace LevelLoaderScripts
{
    public class GameSession
    {
        public string MineId { get; set; }
        public string[] AssetsKey { get; set; }
        public MineData MineData { get; set; }
        public int WarehouseLevel => MineData.WarehouseLevel;
        public int ElevatorLevel => MineData.ElevatorLevel;
    }
    
    public interface IGameSessionProvider
    {
        GameSession GetSession();
    }

    public class GameSessionProvider : IGameSessionProvider
    {
        [Inject] private IDataManager _dataManager;
        [Inject] private GameConfig _config;
        
        public GameSession GetSession()
        {
            var mineId = GetMineId();
            return new GameSession
            {
                MineId = mineId,
                AssetsKey = _config.MinesConfig.GetLevelInformation(mineId).AssetKeys,
                MineData = _dataManager.Get<MinesData>().GetMineData(mineId)
            };
        }

        private string GetMineId()
        {
            var value = _dataManager.Get<GameSessionData>().MineId;
            if (string.IsNullOrEmpty(value))
            {
                value = _config.MinesConfig.DefaultMineInformation.MineId;
            }

            return value;
        }
    }
    
    public interface IGameSessionUpdater
    {
        UniTask UpdateSession(string mineId);
    }
    
    public class GameGameSessionUpdater : IGameSessionUpdater
    {
        [Inject] private IDataManager _dataManager;
        [Inject] private ITimeProvider _timeProvider;

        public async UniTask UpdateSession(string mineId)
        {
            _dataManager.Get<GameSessionData>().MineId = mineId;
            _dataManager.Get<GameSessionData>().UpdateLoggOffTime(mineId, _timeProvider);
            await _dataManager.SaveAllAsync();
        }
    }
}