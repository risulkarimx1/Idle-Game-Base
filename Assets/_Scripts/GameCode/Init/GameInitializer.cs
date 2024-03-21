using System;
using System.Collections.Generic;
using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Mineshaft;
using GameCode.UI;
using GameCode.Warehouse;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameCode.Init
{
    public class GameInitializer: IInitializable, IDisposable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private HudView _hudView;
        [Inject] private CameraView _cameraView;

        [Inject] private ElevatorView _elevatorView;
        [Inject] private WarehouseView _warehouseView;
        [Inject (Id = "FirstMinePosition")] private Transform _mineshaftStartingPosition;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private IMineshaftFactory _mineshaftFactory;
        public void Initialize()
        {
            // _mineshaftFactory.CreateMineshaft(1,1, _mineshaftStartingPosition.position);
            _mineshaftFactory.CreateMineshaftBatch(new Dictionary<int, int>() { { 1, 1 }, { 2, 1 }, { 3, 1 } },
                _mineshaftStartingPosition.position);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}