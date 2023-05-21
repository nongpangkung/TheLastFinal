using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class GoldCountBar : MonoBehaviour
    {
        public TextMeshProUGUI goldCountText;

        public void SetGoldCountText(int goldCount, int increasedGold = 0)
        {
            if (goldCountText == null)
            {
                Debug.LogError("goldCountText reference is null. Make sure it is assigned in the inspector.");
                return;
            }

            // Calculate the new gold count with the increase
            int newGoldCount = goldCount + increasedGold;

            // Display the gold count with the increase in front of the original value
            string displayText = $"{newGoldCount} ({(increasedGold >= 0 ? "+" : "")}{increasedGold})";
            goldCountText.text = displayText;

            // Fade out the increase text over time
            StartCoroutine(FadeOutIncreaseText());
        }

        private IEnumerator FadeOutIncreaseText()
        {
            if (goldCountText == null)
            {
                yield break;
            }

            const float fadeDuration = 2f;
            float elapsedTime = 0f;
            Color originalColor = goldCountText.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                goldCountText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            // Reset the text and color after fading out
            goldCountText.text = goldCountText.text.Split(' ')[0]; // Remove the increase text from the display
            goldCountText.color = originalColor;
        }

    }
}
