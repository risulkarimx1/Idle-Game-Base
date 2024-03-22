using System;
using System.Collections.Generic;
using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Mineshaft;
using GameCode.Persistance;
using GameCode.UI;
using GameCode.Warehouse;
using Services.DataFramework;
using Services.GameInitFramework;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Init
{
    public class GameInitializer: IInitializableAfterAll, IDisposable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private HudView _hudView;
        [Inject] private CameraView _cameraView;

        [Inject] private ElevatorView _elevatorView;
        [Inject] private WarehouseView _warehouseView;
        [Inject (Id = "FirstMinePosition")] private Transform _mineshaftStartingPosition;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private IMineshaftFactory _mineshaftFactory;
        [Inject] private DataManager _dataManager;
        
        public void OnAllInitFinished()
        {
            var mineShaftLevels = _dataManager.Get<GameLevelData>().GetMineShaftLevels();
            _mineshaftFactory.CreateMineshaftBatch(mineShaftLevels, _mineshaftStartingPosition.position);
            
        }

        public async void Dispose()
        {
            await _dataManager.SaveAllAsync();
            _disposable?.Dispose();
        }

        
    }
}