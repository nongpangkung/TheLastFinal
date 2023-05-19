using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public float DestroyTime = 3f;

    private void Start()
    {
        Destroy(gameObject,DestroyTime);
    }
}
