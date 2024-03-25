using Cysharp.Threading.Tasks;
using GameCode.Finance;
using GameCode.Mines;
using GameCode.Tutorial;
using Services.SceneFlowServices;
using UniRx;
using Zenject;

namespace GameCode.UI
{
    public class HudController
    {
        private readonly HudView _view;

        [Inject] private SceneFlowService _sceneFlowService;
        [Inject] private MineSelectionController _mineSelectionController;

        [Inject]
        public HudController(HudView view, FinanceModel financeModel, ITutorialModel tutorialModel,
            CompositeDisposable disposable)
        {
            _view = view;

            financeModel.Money
                .Subscribe(money => view.CashAmount = money)
                .AddTo(disposable);

            tutorialModel.ShouldShowTooltip
                .Subscribe(UpdateTooltipVisibility)
                .AddTo(disposable);

            _view.MapButton.OnClickAsObservable().Subscribe(_ => { _mineSelectionController.ShowAsync().Forget(); })
                .AddTo(disposable);
        }

        private void UpdateTooltipVisibility(bool shouldShowTooltip)
        {
            _view.TooltipVisible = shouldShowTooltip;
        }

        public void ShowPassiveIncomeTooltip(string text)
        {
            _view.ShowPassiveIncomeTooltip(text);
        }
    }
}