using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.Mines
{
    public class MineInfoItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mineNameText;
        [SerializeField] private TextMeshProUGUI mineDescriptionText;
        [SerializeField] private Button goButton;
        [SerializeField] private RectTransform goButtonAnimated;
        [SerializeField] private Color _disableColor;
        [SerializeField] private Color _enableColor;
        [SerializeField] private Image _goButtonImage;
        
        private string _mineId;
        private Action<string> _clickEvent;
        

        public void SetMineInfo(string mineId, string mineName, string mineDescription, Action<string> clickEvent, bool isCurrentMine)
        {
            _mineId = mineId;
            _clickEvent = clickEvent;
            mineNameText.text = mineName;
            mineDescriptionText.text = mineDescription;

            if (isCurrentMine)
            {
                goButton.interactable = false;
                _goButtonImage.color = _disableColor;
            }
            else
            {
                _goButtonImage.color = _enableColor;
                goButton.OnPointerDownAsObservable().Subscribe(_ =>
                    goButtonAnimated.DOAnchorPosY(0, 0.1f).SetEase(Ease.Linear)).AddTo(this);
            
                goButton.OnPointerUpAsObservable().Subscribe(_ => _clickEvent?.Invoke(_mineId)).AddTo(this);   
            }
        }
    }
}