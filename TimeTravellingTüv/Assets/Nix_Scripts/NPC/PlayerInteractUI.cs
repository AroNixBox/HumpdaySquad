using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private Transform containerGameObject;
    [SerializeField] private GameObject keyboardKeyImage;
    [SerializeField] private GameObject controllerKeyImage;

    private void Start()
    {
        keyboardKeyImage.SetActive(!IsControllerConnected());
        controllerKeyImage.SetActive(IsControllerConnected());
    }

    public void Show()
    {
        containerGameObject.gameObject.SetActive(true);
    }

    public void Hide()
    {
        containerGameObject.gameObject.SetActive(false);

    }
    private bool IsControllerConnected()
    {
        string[] joysticks = Input.GetJoystickNames();
        foreach (string joystick in joysticks)
        {
            if (!string.IsNullOrEmpty(joystick))
            {
                return true;
            }
        }
        return false;
    }
}
