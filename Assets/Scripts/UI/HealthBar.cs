using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public TextMeshProUGUI healthText;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            UpdateHealthText(maxHealth, (int)slider.maxValue);
        }

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
            UpdateHealthText(currentHealth, (int)slider.maxValue);
        }

        private void UpdateHealthText(int currentHealth, int maxHealth)
        {
            healthText.text = $"{slider.value} / {slider.maxValue}";
        }
    }
}