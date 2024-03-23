using UnityEngine;

namespace GameCode.Mineshaft
{
    public interface IMineshaftFactory
    {
        MineshaftController CreateMineshaft(string mineId, int mineshaftNumber, int mineshaftLevel, Vector2 position);
    }
}