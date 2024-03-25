using System;
using UnityEngine;

namespace GameCode.Mines
{
    [Serializable]
    public class MinesInformation
    {
        [field: SerializeField] public string MineId { get; private set; }
        [field: SerializeField] public string MineName { get; private set; }

        [field: SerializeField, TextArea(1, 5)]
        public string MineDescription { get; private set; }

        [field: SerializeField] public string[] AssetKeys { get; private set; }
    }
}