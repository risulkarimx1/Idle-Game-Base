using System;
using UnityEngine;

namespace Services.SceneFlowServices
{
    [Serializable]
    public class LevelInformation
    {
        [field: SerializeField] public  string LevelId { get; private set; }
        [field: SerializeField] public string[] AssetKeys { get; private set; }

        public LevelInformation(string levelId, string[] assetKeys)
        {
            LevelId = levelId;
            AssetKeys = assetKeys;
        }
    }
}