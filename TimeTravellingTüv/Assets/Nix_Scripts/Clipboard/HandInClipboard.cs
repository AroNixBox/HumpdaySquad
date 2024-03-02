using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInClipboard : MonoBehaviour, IInteraction
{
    public void Interacter()
    {
        Debug.Log($"You got: {Clipboard.Instance.GetComponentInChildren<UIClipboard>().DocumentPoints()} Points");
    }
}
