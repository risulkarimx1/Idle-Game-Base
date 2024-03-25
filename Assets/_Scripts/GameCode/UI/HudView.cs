using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cashAmount;
        [SerializeField] private TextMeshProUGUI _passiveIncomeText;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private Button _mapButton;

        public double CashAmount
        {
            set
            {
                string formattedValue;
                
                if (value >= 1000000)
                    formattedValue = (value / 1000000).ToString("F1") + "M";
                else
                    formattedValue = value.ToString("F0");
                
                _cashAmount.SetText(formattedValue);
            }
        }

        public bool TooltipVisible
        {
            set => _tooltip.gameObject.SetActive(value);
        }

        public Button MapButton => _mapButton;

        public TextMeshProUGUI PassiveIncomeText => _passiveIncomeText;

        public void ShowPassiveIncomeTooltip(string text)
        {
            _passiveIncomeText.SetText(text);
            _passiveIncomeText.gameObject.SetActive(true);
            var sequence = DOTween.Sequence();
            sequence.Append(_passiveIncomeText.DOFade(0, 0));
            sequence.Append(_passiveIncomeText.DOFade(1, 0.5f));
            sequence.AppendInterval(1);
            sequence.Append(_passiveIncomeText.DOFade(0, 0.5f));
            sequence.AppendCallback(()=> _passiveIncomeText.gameObject.SetActive(false));
        }
    }
}