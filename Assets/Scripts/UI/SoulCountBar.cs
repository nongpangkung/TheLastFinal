using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class SoulCountBar : MonoBehaviour
    {
        public TextMeshProUGUI soulCountText;

        public void SetSoulCountText(int soulCount, int increasedSouls = 0)
        {
            if (soulCountText == null)
            {
                Debug.LogError("soulCountText reference is null. Make sure it is assigned in the inspector.");
                return;
            }

            // Calculate the new soul count with the increase
            int newSoulCount = soulCount + increasedSouls;

            // Display the soul count with the increase in front of the original value
            string displayText = $"{newSoulCount} ({(increasedSouls >= 0 ? "+" : "")}{increasedSouls})";
            soulCountText.text = displayText;

            // Fade out the increase text over time
            StartCoroutine(FadeOutIncreaseText());
        }

        private IEnumerator FadeOutIncreaseText()
        {
            if (soulCountText == null)
            {
                yield break;
            }

            const float fadeDuration = 2f;
            float elapsedTime = 0f;
            Color originalColor = soulCountText.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                soulCountText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            // Reset the text and color after fading out
            soulCountText.text = soulCountText.text.Split(' ')[0]; // Remove the increase text from the display
            soulCountText.color = originalColor;
        }
    }
}
