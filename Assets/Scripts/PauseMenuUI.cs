using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public Button retry;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(retry.gameObject);
    }
}
