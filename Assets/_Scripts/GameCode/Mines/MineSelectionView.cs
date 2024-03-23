using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.Mines
{
    public class MineSelectionView: MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private RectTransform mineSelectionUi;
        
        [SerializeField] private Transform contentParent;
        [SerializeField] private MineInfoItemView mineInfoItemViewPrefab;

        public Transform ContentParent => contentParent;

        public MineInfoItemView MineInfoItemViewPrefab => mineInfoItemViewPrefab;

        private void Awake()
        {
            background.gameObject.SetActive(false);
        }
        public UniTask ShowMineSelectionUiFlowAsync()
        {
            background.gameObject.SetActive(true);
            UniTask.Yield();
            mineSelectionUi.DOAnchorPosX(-600, 0);
            var sequence = DOTween.Sequence();
            sequence.Append(background.DOFade(0.5f, 0.1f));
            sequence.Append(mineSelectionUi.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutCubic));
            return sequence.ToUniTask();
        }
        
        public UniTask HideMineSelectionUiFlow()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(mineSelectionUi.DOAnchorPosX(-600, 0.2f).SetEase(Ease.InCubic));
            sequence.Append(background.DOFade(0, 0.1f));
            sequence.OnComplete(() => background.gameObject.SetActive(false));
            return sequence.ToUniTask();
        }
    }
}