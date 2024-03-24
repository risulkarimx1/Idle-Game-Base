using GameCode.Elevator;
using GameCode.Persistence;
using GameCode.Signals;
using GameCode.Warehouse;
using LevelLoaderScripts;
using Services.GameInitFramework;
using UniRx;
using Zenject;

namespace GameCode.Mineshaft
{
    public class MineDataService : IInitializableAfterAll
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private WarehouseModel _warehouseModel;
        [Inject] private ElevatorModel _elevatorModel;
        [Inject] private GameSessionProvider _sessionProvider;
        private MineData _mineData;

        public void OnAllInitFinished()
        {
            _mineData = _sessionProvider.SessionMineData();
            
            _signalBus.GetStream<GameSignals.MineshaftCreatedSignal>().Subscribe(OnMineShaftCreated).AddTo(_disposable);

            _warehouseModel.Level.Subscribe(value =>
                {
                    _mineData.WarehouseLevel = value;
                })
                .AddTo(_disposable);

            _elevatorModel.Level.Subscribe(value =>
            {
                _mineData.ElevatorLevel = value;
            }).AddTo(_disposable);
        }

        private void OnMineShaftCreated(GameSignals.MineshaftCreatedSignal signal)
        {
            signal.MineshaftModel.Level.Subscribe(value =>
            {
                _mineData.SetMineShaftLevel(signal.MineshaftNumber, value);
            }).AddTo(_disposable);
        }
    }
}