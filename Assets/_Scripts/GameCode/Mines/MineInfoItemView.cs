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
        [SerializeField] private Color disableColor;
        [SerializeField] private Color enableColor;
        [SerializeField] private Image goButtonImage;
        
        public void Configure(string mineId, string mineName, string mineDescription, bool isCurrentMine, Action<string> mineSelected)
        {
            mineNameText.text = mineName;
            mineDescriptionText.text = mineDescription;

            if (isCurrentMine)
            {
                goButton.interactable = false;
                goButtonImage.color = disableColor;
            }
            else
            {
                goButtonImage.color = enableColor;
                goButton.interactable = true;
                
                goButton.OnPointerDownAsObservable().Subscribe(_ =>
                    goButtonAnimated.DOAnchorPosY(0, 0.1f).SetEase(Ease.Linear)).AddTo(this);
                
                goButton.OnPointerUpAsObservable().Subscribe(_ =>
                {
                    mineSelected?.Invoke(mineId);
                    goButton.interactable = false;
                }).AddTo(this);   
            }
        }
    }
}
