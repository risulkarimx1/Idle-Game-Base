using System.Collections.Generic;
using Services.LogFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = Services.LogFramework.Debug;

namespace Services.SceneFlowServices
{
    [CreateAssetMenu(fileName = "Levels Config", menuName = "Levels Config")]
    public class LevelsConfig : SerializedScriptableObject
    {
        [SerializeField] private string[] defaultAssetsKey;

        [SerializeField]
        private Dictionary<string, string[]> _levelsToAssetsMap;
        
        public string[] GetLevelInformation(string levelId)
        {
            if (_levelsToAssetsMap.TryGetValue(levelId, out var levelInformation))
                return levelInformation;

            Debug.LogError($"Assets Key with id {levelId} not found in the levels config", LogContext.LevelConfig);
            return defaultAssetsKey;
        }
    }
}