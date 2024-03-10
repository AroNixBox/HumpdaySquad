using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelecter : MonoBehaviour
{
    [SerializeField] private GameObject eSystem;
    [SerializeField] private GameObject Button;
    private EventSystem _system;

    private void Start()
    {
        _system = eSystem.GetComponent<EventSystem>();
    }

    public void OnSelectButton()
    {
        _system.firstSelectedGameObject = Button;
        _system.SetSelectedGameObject(Button);
    }
}
