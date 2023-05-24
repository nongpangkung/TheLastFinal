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

        public void SetSoulCountText(int soulCount, int increasedSouls = 0, float displayDuration = 2f)
        {
            if (soulCountText == null)
            {
                Debug.LogError("soulCountText reference is null. Make sure it is assigned in the inspector.");
                return;
            }

            // Calculate the new soul count with the increase
            int newSoulCount = soulCount + increasedSouls;

            // Display the soul count with the increase
            string displayText = $"{newSoulCount} ({(increasedSouls >= 0 ? "+" : "")}{increasedSouls})";
            soulCountText.text = displayText;

            // Start the coroutine to fade out the increase text after the specified duration
            StartCoroutine(FadeOutIncreaseText(displayDuration));
        }

        private IEnumerator FadeOutIncreaseText(float duration)
        {
            if (soulCountText == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(duration);

            // Reset the text to the original soul count without the increase
            int soulCount = int.Parse(soulCountText.text.Split(' ')[0]);
            soulCountText.text = soulCount.ToString();
        }
    }
}
