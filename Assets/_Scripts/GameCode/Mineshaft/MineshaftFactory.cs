using GameCode.Finance;
using GameCode.Init;
using GameCode.Mineshaft;
using GameCode.Signals;
using Services.SceneFlowServices;
using UniRx;
using UnityEngine;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class MineshaftFactory : IMineshaftFactory
{
    [Inject] private MineshaftCollectionModel _collectionModel;
    [Inject] private FinanceModel _financeModel;
    [Inject] private GameConfig _config;
    [Inject] private CompositeDisposable _disposable;
    [Inject] private SignalBus _signalBus;

    public MineshaftController CreateMineshaft(string mineId, int mineshaftNumber, int mineshaftLevel, Vector2 position)
    {
        var view = GameObject.Instantiate(_config.MineshaftConfig.MineshaftPrefab, position, Quaternion.identity);
        SceneFlowService.MoveObjectToScene(view.gameObject, GameConfig.GetInstance().GameScene);
        var mineshaftModel = new MineshaftModel(mineshaftNumber, mineshaftLevel, _config, _financeModel, _disposable);
        var controller = new MineshaftController(view, mineshaftModel, this, _config, _disposable, mineId);
        _collectionModel.RegisterMineshaft(mineshaftNumber, mineshaftModel, view);
        _signalBus.Fire(new GameSignals.MineshaftCreatedSignal(mineId, mineshaftNumber, mineshaftModel, position));
        return controller;
    }
}