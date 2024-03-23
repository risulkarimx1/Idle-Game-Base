using System;
using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Mineshaft;
using GameCode.Persistence;
using GameCode.UI;
using GameCode.Utils;
using GameCode.Warehouse;
using Services.DataFramework;
using Services.GameInitFramework;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Init
{
    public class GameInitializer : IInitializableAfterAll, IDisposable
    {
        [Inject(Id = GameConstants.FirtMinePositionObjectTag)] private Transform _mineshaftStartingPosition;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private IMineshaftFactory _mineshaftFactory;
        [Inject] private DataManager _dataManager;

        public void OnAllInitFinished()
        {
            var mineId = "mine_1";
            var minesData = _dataManager.Get<MinesData>();
            var mineShaftLevels = minesData.ReadMineshaftLevels(mineId);
            
            var mineShaftPosition = _mineshaftStartingPosition.position;
            
            foreach (var mineShaftLevel in mineShaftLevels)
            {
               var controller = _mineshaftFactory.CreateMineshaft(mineId, mineShaftLevel.Key, mineShaftLevel.Value, mineShaftPosition);
               mineShaftPosition = controller.View.NextShaftView.NextShaftPosition;
            }
        }

        public async void Dispose()
        {
            await _dataManager.SaveAllAsync();
            _disposable?.Dispose();
        }
    }
}