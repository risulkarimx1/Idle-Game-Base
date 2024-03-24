using GameCode.Mines;
using GameCode.Mineshaft;
using GameCode.Worker;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCode.Init
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [BoxGroup("Constants")] [field: SerializeField] public float ActorUpgradePriceIncrement { get; private set; }
        [BoxGroup("Constants")] [field: SerializeField] public float ActorUpgradeSkillIncrement { get; private set; }

        [BoxGroup("Constants")] [field: SerializeField] public float ActorPriceIncrementPerShaft { get; private set; }
        [BoxGroup("Constants")] [field: SerializeField] public float ActorSkillIncrementPerShaft { get; private set; }
        [BoxGroup("Constants")] [field: SerializeField] public int StartingMoney { get; private set; }
        
        [BoxGroup("Configs")] [field: SerializeField] public MineshaftConfig MineshaftConfig { get; private set; }
        [BoxGroup("Configs")] [field: SerializeField] public WorkerConfig MineshaftWorkerConfig{ get; private set; }
        [BoxGroup("Configs")] [field: SerializeField] public WorkerConfig ElevatorWorkerConfig{ get; private set; }
        [BoxGroup("Configs")] [field: SerializeField] public WorkerConfig WarehouseWorkerConfig { get; private set; }
        [BoxGroup("Configs")] [field: SerializeField] public MinesConfig MinesConfig{ get; private set; }
    }
}