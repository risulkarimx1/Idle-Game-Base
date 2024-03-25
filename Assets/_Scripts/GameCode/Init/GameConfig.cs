using GameCode.Mines;
using GameCode.Mineshaft;
using GameCode.Worker;
using Services.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCode.Init
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : SingletonScriptable<GameConfig>
    {
        [field: BoxGroup("Data Encryption")]
        [field: SerializeField]
        public string DataKey { get; private set; } = "1234567890abcdef1234567890abcdef";

        [field: BoxGroup("Data Encryption")]
        [field: SerializeField]
        public string DataInitVector { get; private set; } = "1234567890abcdef";

        [field: BoxGroup("Scene Names")]
        [field: SerializeField]
        public string BootScene { get; private set; } = "BootScene";

        [field: BoxGroup("Scene Names")]
        [field: SerializeField]
        public string GameScene { get; private set; } = "GameScene";

        [field: BoxGroup("Scene Names")]
        [field: SerializeField]
        public string SessionLoaderScene { get; private set; } = "SessionLoaderScene";

        [field: BoxGroup("Passive Income")]
        [field: SerializeField]
        public bool EnablePassiveIncome { get; private set; }

        [field: BoxGroup("Passive Income")]
        [field: SerializeField]
        public double MaximumPassiveIncome { get; private set; } = 100000;

        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public double MineShaftBasePrice { get; private set; } = 60;
        
        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public float ActorUpgradePriceIncrement { get; private set; }

        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public float ActorUpgradeSkillIncrement { get; private set; }

        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public float ActorPriceIncrementPerShaft { get; private set; }

        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public float ActorSkillIncrementPerShaft { get; private set; }

        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public double WareHouseBasePrice { get; private set; } = 60;

        [field: BoxGroup("Game Settings")]
        [field: SerializeField]
        public double ElevatorBasePrice { get; private set; } = 60;

        [field: BoxGroup("Game Configs")]
        [field: SerializeField]
        public MineshaftConfig MineshaftConfig { get; private set; }

        [field: BoxGroup("Game Configs")]
        [field: SerializeField]
        public WorkerConfig MineshaftWorkerConfig { get; private set; }

        [field: BoxGroup("Game Configs")]
        [field: SerializeField]
        public WorkerConfig ElevatorWorkerConfig { get; private set; }

        [field: BoxGroup("Game Configs")]
        [field: SerializeField]
        public WorkerConfig WarehouseWorkerConfig { get; private set; }

        [field: BoxGroup("Game Configs")]
        [field: SerializeField]
        public MinesConfig MinesConfig { get; private set; }
    }
}