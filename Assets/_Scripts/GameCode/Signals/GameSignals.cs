using GameCode.Mineshaft;
using UnityEngine;

namespace GameCode.Signals
{
    public class GameSignals
    {
        public class MineshaftCreatedSignal
        {
            public string MineId { get; private set; }
            public int MineshaftNumber { get; private set; }
            public MineshaftModel MineshaftModel { get; private set; }
            public Vector2 Position { get; private set; }

            public MineshaftCreatedSignal(string mineId, int mineshaftNumber, MineshaftModel mineshaftModel,
                Vector2 position)
            {
                MineId = mineId;
                MineshaftNumber = mineshaftNumber;
                MineshaftModel = mineshaftModel;
                Position = position;
            }
        }

        public class DepositSignal
        {
            public double Amount { get; private set; }

            public DepositSignal(double amount)
            {
                Amount = amount;
            }
        }
    }
}