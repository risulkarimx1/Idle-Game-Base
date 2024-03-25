using GameCode.Persistence;

namespace GameSessions
{
    public class GameSession
    {
        public string MineId { get; set; }
        public string[] AssetsKey { get; set; }
        public MineData MineData { get; set; }
        public int WarehouseLevel => MineData.WarehouseLevel;
        public int ElevatorLevel => MineData.ElevatorLevel;
    }
}