using GameCode.Init;
using GameCode.Persistence;
using Services.DataFramework;
using Zenject;

namespace GameSessions
{
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
            var session = new GameSession
            {
                MineId = mineId,
                AssetsKey = _config.MinesConfig.GetLevelInformation(mineId).AssetKeys,
                MineData = _dataManager.Get<MinesData>().GetMineData(mineId)
            };
            return session;
        }

        private string GetMineId()
        {
            var gameSession = _dataManager.Get<GameSessionData>();
            if (gameSession == null || string.IsNullOrEmpty(gameSession.MineId))
            {
                return _config.MinesConfig.DefaultMineInformation.MineId;
            }

            return gameSession.MineId;
        }
    }
}