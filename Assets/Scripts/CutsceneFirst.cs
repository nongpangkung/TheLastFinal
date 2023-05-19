using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneFirst : MonoBehaviour
{
    public void EnablePanel()
    {
        gameObject.SetActive(true);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
