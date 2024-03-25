using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Services.LoadingScreen
{
    public class LoadingController : IInitializable
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
    }
}