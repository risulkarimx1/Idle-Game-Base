using System;
using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Finance;
using GameCode.Mineshaft;
using GameCode.Tutorial;
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
        private CompositeDisposable _disposable;
        public void Initialize()
        {
            _disposable = new CompositeDisposable();

            var tutorialModel = new TutorialModel();
            var financeModel = new FinanceModel();
            
            new CameraController(_cameraView, tutorialModel);

            //Hud
            new HudController(_hudView, financeModel, tutorialModel, _disposable);

            //Mineshaft
            var mineshaftCollectionModel = new MineshaftCollectionModel();
            var mineshaftFactory = new MineshaftFactory(mineshaftCollectionModel, financeModel, _gameConfig, _disposable);
            mineshaftFactory.CreateMineshaft(1,1, _mineshaftStartingPosition.position);

            //Elevator
            var elevatorModel = new ElevatorModel(1, _gameConfig, financeModel, _disposable);
            new ElevatorController(_elevatorView, elevatorModel, mineshaftCollectionModel, _gameConfig, _disposable);
            
            //Warehouse
            var warehouseModel = new WarehouseModel(1, _gameConfig, financeModel, _disposable);
            new WarehouseController(_warehouseView, warehouseModel, elevatorModel, _gameConfig, _disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}