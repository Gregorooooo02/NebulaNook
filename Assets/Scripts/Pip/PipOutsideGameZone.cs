using System;
using UnityEngine;

public class PipOutsideGameZone : MonoBehaviour
{
    [SerializeField] private PipScript pipScript;

    void OnTriggerExit(Collider other)
    {
        pipScript.ResetPosition();
    }
}
