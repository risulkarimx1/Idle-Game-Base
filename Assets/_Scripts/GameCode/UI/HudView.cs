using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cashAmount;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _mapButton;

        public double CashAmount
        {
            set => _cashAmount.SetText(value.ToString("F0"));
        }

        public bool TooltipVisible
        {
            set => _tooltip.gameObject.SetActive(value);
        }

        public Button ResetButton => _resetButton;

        public Button MapButton => _mapButton;
    }
}