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

        public void SetGoldCountText(int goldCount, int increasedGold = 0, float displayDuration = 2f)
        {
            if (goldCountText == null)
            {
                Debug.LogError("goldCountText reference is null. Make sure it is assigned in the inspector.");
                return;
            }

            // Calculate the new gold count with the increase
            int newGoldCount = goldCount + increasedGold;

            // Display the gold count with the increase
            string displayText = $"{newGoldCount} ({(increasedGold >= 0 ? "+" : "")}{increasedGold})";
            goldCountText.text = displayText;

            // Start the coroutine to fade out the increase text after the specified duration
            StartCoroutine(FadeOutIncreaseText(displayDuration));
        }

        private IEnumerator FadeOutIncreaseText(float duration)
        {
            if (goldCountText == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(duration);

            // Reset the text to the original gold count without the increase
            int goldCount = int.Parse(goldCountText.text.Split(' ')[0]);
            goldCountText.text = goldCount.ToString();
        }
    }
}
