using GameCode.Elevator;
using GameCode.GameArea;
using GameCode.Init;
using GameCode.Persistence;
using GameCode.Signals;
using GameCode.Utils;
using GameCode.Warehouse;
using Services.DataFramework;
using Services.GameInitFramework;
using UniRx;
using Zenject;

namespace GameCode.Mineshaft
{
    public class MineDataService : IInitializableAfter<DataManager>
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;
        // [Inject (Id = GameConstants.WarehouseModelId)] private WarehouseModel _warehouseModel;
        // [Inject (Id = GameConstants.ElevatorModelId)] private ElevatorModel _elevatorModel;
        [Inject] private WarehouseModel _warehouseModel;
        [Inject] private ElevatorModel _elevatorModel;

        private MinesData _minesData;

        public void OnInitFinishedFor(DataManager dataManager)
        {
            _minesData = dataManager.Get<MinesData>();
            _signalBus.GetStream<GameSignals.MineshaftCreatedSignal>().Subscribe(OnMineShaftCreated).AddTo(_disposable);
            
            _warehouseModel.Level.Subscribe(value =>
            {
                _minesData.SetWarehouseLevel("mine_1", value); // TODO: Should come from MineSelectionController
            }).AddTo(_disposable);
            
            _elevatorModel.Level.Subscribe(value =>
            {
                _minesData.SetElevatorLevel("mine_1", value); // TODO: Should come from MineSelectionController
            }).AddTo(_disposable);
        }

        private void OnMineShaftCreated(GameSignals.MineshaftCreatedSignal signal)
        {
            signal.MineshaftModel.Level.Subscribe(value =>
            {
                _minesData.SetMineShaftLevel(signal.MineId, signal.MineshaftNumber, value);
            }).AddTo(_disposable);
        }
    }
}