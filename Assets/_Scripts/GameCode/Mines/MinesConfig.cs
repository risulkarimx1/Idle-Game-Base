using System.Collections.Generic;
using Services.LogFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = Services.LogFramework.Debug;

namespace GameCode.Mines
{
    [CreateAssetMenu(fileName = "Mines Config", menuName = "Mines Config")]
    public class MinesConfig : SerializedScriptableObject
    {
        [SerializeField] private MinesInformation defaultMineInformation;

        [SerializeField] private Dictionary<string, MinesInformation> _minesInformation;

        public Dictionary<string, MinesInformation> MinesInformation => _minesInformation;

        public MinesInformation DefaultMineInformation => defaultMineInformation;

        public MinesInformation GetLevelInformation(string mineId)
        {
            if (MinesInformation.TryGetValue(mineId, out MinesInformation mineInformation))
                return mineInformation;

            Debug.LogError($"Assets Key with id {mineId} not found in the levels config", LogContext.LevelConfig);
            return DefaultMineInformation;
        }
    }
}