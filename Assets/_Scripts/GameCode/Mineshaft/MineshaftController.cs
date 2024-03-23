using GameCode.Init;
using GameCode.Worker;
using UniRx;

namespace GameCode.Mineshaft
{
    public class MineshaftController
    {
        private readonly MineshaftView _view;
        private readonly MineshaftModel _model;
        public MineshaftView View => _view;
        public MineshaftModel Model => _model;
        
        private readonly IMineshaftFactory _mineshaftFactory;
        private readonly string _mineId;
        
        public MineshaftController(MineshaftView view, MineshaftModel model, IMineshaftFactory mineshaftFactory,
            GameConfig gameConfig, CompositeDisposable disposable, string mineId)
        {
            _view = view;
            _model = model;
            _mineshaftFactory = mineshaftFactory;
            _mineId = mineId;
            var workerModel = new WorkerModel(model, gameConfig.MineshaftWorkerConfig, disposable);
            new MineshaftWorkerController(view, model, workerModel, disposable);

            model.CanUpgrade
                .Subscribe(canUpgrade => view.AreaUiCanvasView.UpgradeButton.interactable = canUpgrade)
                .AddTo(disposable);

            view.AreaUiCanvasView.UpgradeButton.OnClickAsObservable()
                .Subscribe(_ => Upgrade())
                .AddTo(disposable);

            model.StashAmount
                .Subscribe(amount => view.StashAmount = amount.ToString("F0"))
                .AddTo(disposable);
            workerModel.CarryingCapacity
                .Subscribe(capacity => view.AreaUiCanvasView.CarryingCapacity = capacity.ToString("F0"))
                .AddTo(disposable);

            model.UpgradePrice
                .Subscribe(upgradePrice => view.AreaUiCanvasView.UpgradeCost = upgradePrice.ToString("F0"))
                .AddTo(disposable);

            view.NextShaftView.Cost = model.NextShaftPrice.ToString("F0");
            var canBuyNextShaft = model.CanBuyNextShaft.ToReactiveCommand();
            canBuyNextShaft.BindTo(view.NextShaftView.Button).AddTo(disposable);
            
            canBuyNextShaft.Subscribe(_ => BuyNextShaft())
                .AddTo(disposable);
        }

        private void Upgrade()
        {
            Model.Upgrade();
        }

        private void BuyNextShaft()
        {
            Model.BuyNextShaft();
            View.NextShaftView.Visible = false;
            _mineshaftFactory.CreateMineshaft(_mineId, Model.MineshaftNumber + 1, 1, View.NextShaftView.NextShaftPosition);
        }
    }
}