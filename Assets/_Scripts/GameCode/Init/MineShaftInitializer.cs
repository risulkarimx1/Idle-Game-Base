using GameCode.Mineshaft;
using GameCode.Persistence;
using LevelLoaderScripts;
using Services.DataFramework;
using Services.GameInitFramework;
using Services.Utils;
using UnityEngine;
using Zenject;

namespace GameCode.Init
{
    public class MineShaftInitializer: IInitializableAfterAll
    {
        [Inject(Id = GameConstants.FirstMinePositionObjectTag)]
        private Transform _mineshaftStartingPosition;
        
        [Inject] private IMineshaftFactory _mineshaftFactory;
        [Inject] private IDataManager _dataManager;
        [Inject] private IGameSessionProvider _gameSessionProvider;
        
        public void OnAllInitFinished()
        {
            SetupMineShafts();
        }
        private void SetupMineShafts()
        {
            var mineId = _gameSessionProvider.GetSession().MineId;
            var minesData = _dataManager.Get<MinesData>();
            var mineShaftLevels = minesData.ReadMineshaftLevels(mineId);
            var mineShaftPosition = _mineshaftStartingPosition.position;

            foreach (var mineShaftLevel in mineShaftLevels)
            {
                var controller = _mineshaftFactory.CreateMineshaft(mineId, mineShaftLevel.Key, mineShaftLevel.Value,
                    mineShaftPosition);
                mineShaftPosition = controller.View.NextShaftView.NextShaftPosition;
            }
        }
    }
}