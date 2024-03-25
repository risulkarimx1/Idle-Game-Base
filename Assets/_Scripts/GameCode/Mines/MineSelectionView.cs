using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.Mines
{
    public class MineSelectionView: MonoBehaviour
    {
        [SerializeField] private RectTransform containerParent; 
        [SerializeField] private RectTransform mineSelectionUi;
        
        [SerializeField] private Transform contentParent;
        [SerializeField] private MineInfoItemView mineInfoItemViewPrefab;

        [SerializeField] private Button closeButton;
        [SerializeField] private Button backdropButton;
        
        public Transform ContentParent => contentParent;

        public MineInfoItemView MineInfoItemViewPrefab => mineInfoItemViewPrefab;

        public Button BackdropButton => backdropButton;
        private Image _background;
        public Button CloseButton => closeButton;

        private float _offScreenPosX;
        private void Awake()
        {
            containerParent.gameObject.SetActive(false);
            _background = backdropButton.GetComponent<Image>();
            var uiWidth = mineSelectionUi.rect.width; 
            _offScreenPosX = -(Screen.width / 2 + uiWidth / 2);
        }
        public UniTask ShowMineSelectionUiFlowAsync()
        {
            containerParent.gameObject.SetActive(true);
            mineSelectionUi.DOAnchorPosX(_offScreenPosX, 0);
            UniTask.Yield();
            var sequence = DOTween.Sequence();
            sequence.Append(_background.DOFade(0.5f, 0.1f));
            sequence.Append(mineSelectionUi.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutCubic));
            return sequence.ToUniTask();
        }
        
        public UniTask HideMineSelectionUiFlow()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(mineSelectionUi.DOAnchorPosX(_offScreenPosX, 0.2f).SetEase(Ease.InCubic));
            sequence.Append(_background.DOFade(0, 0.1f));
            sequence.OnComplete(() => containerParent.gameObject.SetActive(false));
            return sequence.ToUniTask();
        }
    }
}