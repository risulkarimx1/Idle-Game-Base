using System.Collections.Generic;
using UnityEngine;

namespace GameCode.Mineshaft
{
    public interface IMineshaftFactory
    {
        void CreateMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position);
        public void CreateMineshaftBatch(Dictionary<int, int> mineshaftLevels, Vector2 position);
    }
}