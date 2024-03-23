using GameCode.Mineshaft;
using UnityEngine;

namespace GameCode.Signals
{
    public class GameSignals
    {
        public class MineshaftCreatedSignal
        {
            public int MineshaftNumber { get; private set; }
            public MineshaftModel MineshaftModel { get; private set; }
            public Vector2 Position { get; private set; }

            public MineshaftCreatedSignal(int mineshaftNumber, MineshaftModel mineshaftModel, Vector2 position)
            {
                MineshaftNumber = mineshaftNumber;
                MineshaftModel = mineshaftModel;
                Position = position;
            }
        }
    }
}