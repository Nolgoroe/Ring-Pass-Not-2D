using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Disable", 2);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
