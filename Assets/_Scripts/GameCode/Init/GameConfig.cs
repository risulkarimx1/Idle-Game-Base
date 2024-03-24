﻿using GameCode.Mines;
using GameCode.Mineshaft;
using GameCode.Worker;
using UnityEngine;

namespace GameCode.Init
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public bool EnablePassiveIncome { get; set; }
        [field: SerializeField] public float ActorUpgradePriceIncrement { get; private set; }
        [field: SerializeField] public float ActorUpgradeSkillIncrement { get; private set; }

        [field: SerializeField] public float ActorPriceIncrementPerShaft { get; private set; }
        [field: SerializeField] public float ActorSkillIncrementPerShaft { get; private set; }
        [field: SerializeField] public int StartingMoney { get; private set; }

        [field: SerializeField] public MineshaftConfig MineshaftConfig { get; private set; }
        [field: SerializeField] public WorkerConfig MineshaftWorkerConfig { get; private set; }
        [field: SerializeField] public WorkerConfig ElevatorWorkerConfig { get; private set; }
        [field: SerializeField] public WorkerConfig WarehouseWorkerConfig { get; private set; }
        [field: SerializeField] public MinesConfig MinesConfig { get; private set; }
    }
}