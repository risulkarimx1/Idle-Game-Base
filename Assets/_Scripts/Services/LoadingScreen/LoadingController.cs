using Cysharp.Threading.Tasks;
using LoadingSceneScripts;
using UniRx;
using Zenject;

namespace Frameworks.LoadingScreen
{
    public class LoadingController
    {
        [Inject] private LoadingView _loadingView;
        private ReactiveProperty<float> _loadingProgress;
        
        public async UniTask Update(float value)
        {
            if (_loadingProgress == null)
            {
                var disposable = CreateLoadingProgressStream();
                
                await _loadingView.Appear();
                await _loadingProgress.Where(x => x >= 1.0f).First().ToUniTask();
                await _loadingView.Disappear();
                
                _loadingProgress?.Dispose();
                _loadingProgress = null;
                
                disposable.Dispose();
            }
            else
            {
                _loadingProgress.Value = value;
            }
            
        }

        private CompositeDisposable CreateLoadingProgressStream()
        {
            var disposable = new CompositeDisposable();
            _loadingProgress = new ReactiveProperty<float>(0).AddTo(disposable);
            _loadingProgress.Value = 0;

            _loadingProgress
                .Subscribe(async progress => { _loadingView.FillupBar.fillAmount = progress; })
                .AddTo(disposable);
            return disposable;
        }
    }
}