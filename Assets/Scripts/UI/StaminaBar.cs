using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;
        public TextMeshProUGUI staminaText;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
            UpdateStaminaText();
        }

        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
            UpdateStaminaText();
        }

        private void UpdateStaminaText()
        {
            int currentValue = Mathf.RoundToInt(slider.value);
            int maxValue = Mathf.RoundToInt(slider.maxValue);
            staminaText.text = $"{currentValue} / {maxValue}";
        }
    }
}