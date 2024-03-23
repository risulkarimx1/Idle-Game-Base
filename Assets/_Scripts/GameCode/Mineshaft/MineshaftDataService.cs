using GameCode.Persistance;
using GameCode.Signals;
using Services.DataFramework;
using Services.GameInitFramework;
using UniRx;
using Zenject;

namespace GameCode.Mineshaft
{
    public class MineshaftDataService : IInitializableAfter<DataManager>
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;

        private GameLevelData _gameLevelData;

        public void OnInitFinishedFor(DataManager dataManager)
        {
            _gameLevelData = dataManager.Get<GameLevelData>();
            _signalBus.GetStream<GameSignals.MineshaftCreatedSignal>().Subscribe(OnMineShaftCreated).AddTo(_disposable);
        }

        private void OnMineShaftCreated(GameSignals.MineshaftCreatedSignal signal)
        {
            signal.MineshaftModel.Level.Subscribe(value =>
            {
                _gameLevelData.SetMineshaftLevel(signal.MineshaftNumber, value);
            }).AddTo(_disposable);
        }
    }
}