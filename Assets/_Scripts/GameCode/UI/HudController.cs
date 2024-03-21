using Cysharp.Threading.Tasks;
using GameCode.Finance;
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
            _view.ResetButton.OnClickAsObservable().Subscribe(_ =>
            {
                 _sceneFlowService.SwitchScene(SceneFlowService.LevelLoaderScene, true).Forget();
            }).AddTo(disposable);
        }

        private void UpdateTooltipVisibility(bool shouldShowTooltip)
        {
            _view.TooltipVisible = shouldShowTooltip;
        }
    }
}