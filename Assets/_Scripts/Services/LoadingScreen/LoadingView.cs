using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Services.LoadingScreen
{
    public class LoadingView : MonoBehaviour
    {
        [field: SerializeField] public Image Background { get; private set; }
        [field: SerializeField] public Image FillupBar { get; private set; }

        public async UniTask Appear()
        {
            FillupBar.fillAmount = 0;
            await Background.DOFade(1, 0.5f).ToUniTask();
        }

        public async UniTask Disappear()
        {
            FillupBar.fillAmount = 0;
            await Background.DOFade(0, 0.5f).ToUniTask();
        }
    }
}
