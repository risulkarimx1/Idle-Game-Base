using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Services.LoadingScreen
{
    public class LoadingView : MonoBehaviour
    {
        [field: SerializeField] public Image Background { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LoadingText { get; private set; }

        private void OnEnable()
        {
            Background.color = new Color(0, 0, 0, 0);
        }

        public async UniTask Appear()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(Background.DOFade(1, 0.5f));
            sequence.Join(LoadingText.DOFade(1, 0.5f));
            await sequence.ToUniTask();
        }

        public async UniTask Disappear()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(LoadingText.DOFade(0, 0.4f));
            sequence.Join(Background.DOFade(0, 0.5f));
            await sequence.ToUniTask();
        }
    }
}
