using System.Collections.Generic;
using JetBrains.Annotations;
using Services.LogFramework;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Debug = Services.LogFramework.Debug;

namespace Services.SceneFlowServices
{
    [CreateAssetMenu(fileName = "Levels Config", menuName = "Levels Config")]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private string[] defaultAssetsKey;

        [ShowInInspector, OnValueChanged(nameof(OnLevelsToAssetsMapChanged))]
        private Dictionary<string, string[]> _levelsToAssetsMap = new();

        [UsedImplicitly]
        private void OnLevelsToAssetsMapChanged()
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            #endif
        }
        public string[] GetLevelInformation(string levelId)
        {
            if (_levelsToAssetsMap.TryGetValue(levelId, out var levelInformation))
                return levelInformation;

            Debug.LogError($"Assets Key with id {levelId} not found in the levels config", LogContext.LevelConfig);
            return defaultAssetsKey;
        }
    }
}