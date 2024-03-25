using Cysharp.Threading.Tasks;
using GameCode.Persistence;
using GameCode.TimeProvider;
using Services.DataFramework;
using Zenject;

namespace GameSessions
{
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