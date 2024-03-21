using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Services.LoadingScreen
{
    public class LoadingController: IInitializable
    {
        [Inject] private LoadingView _loadingView;
        private ReactiveProperty<float> _loadingProgress;
        
        public void Initialize()
        {
            _loadingView.gameObject.SetActive(false);   
        }

        public async UniTask Appear()
        {
            _loadingView.gameObject.SetActive(true);  
            await _loadingView.Appear();
        }
        public async UniTask Hide()
        {
            await _loadingView.Disappear();
            _loadingView.gameObject.SetActive(false);
        }
        // public async UniTask Update(float value)
        // {
        //     if (_loadingProgress == null)
        //     {
        //         _loadingView.gameObject.SetActive(true);
        //         var disposable = CreateLoadingProgressStream();
        //         
        //         await _loadingView.Appear();
        //         // await _loadingProgress.Where(x => x >= 1.0f).First().ToUniTask();
        //         await _loadingView.Disappear();
        //         
        //         _loadingProgress?.Dispose();
        //         _loadingProgress = null;
        //         
        //         disposable.Dispose();
        //         _loadingView.gameObject.SetActive(false);
        //     }
        //     else
        //     {
        //         _loadingProgress.Value = value;
        //     }
        //     
        // }

        // private CompositeDisposable CreateLoadingProgressStream()
        // {
        //     var disposable = new CompositeDisposable();
        //     _loadingProgress = new ReactiveProperty<float>(0).AddTo(disposable);
        //     _loadingProgress.Value = 0;
        //
        //     _loadingProgress
        //         .Subscribe(async progress => { _loadingView.FillupBar.fillAmount = progress; })
        //         .AddTo(disposable);
        //     return disposable;
        // }
    }
}