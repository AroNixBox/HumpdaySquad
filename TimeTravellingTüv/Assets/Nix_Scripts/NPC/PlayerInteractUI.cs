using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private Transform containerGameObject;

    public void Show()
    {
        containerGameObject.gameObject.SetActive(true);
    }

    public void Hide()
    {
        containerGameObject.gameObject.SetActive(false);

    }
}
