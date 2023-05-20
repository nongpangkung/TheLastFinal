using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldPopup : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;

    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        StartCoroutine(PopupRoutine());
    }

    private IEnumerator PopupRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0f, 1f, 0f); // Adjust the Y value as needed for the desired popup height

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            textMesh.alpha = 1f - t;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
