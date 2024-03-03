using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject eSystem;
    [SerializeField] private GameObject Button;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Pause1;
    [SerializeField] private GameObject Pause2;
    [SerializeField] private GameObject Pause3;
    [SerializeField] private GameObject Pause4;
    private FirstPersonController _FPS;
    private StarterAssetsInputs _starterAssetsInputs;
    private Interact _interact;
    private EventSystem _eventSystem;
    private void Start()
    {
        _FPS = GetComponent<FirstPersonController>();
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        _interact = FindObjectOfType<Interact>();
        _eventSystem = FindObjectOfType<EventSystem>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_starterAssetsInputs.menu && _interact.IsInteracting)
        {
            _starterAssetsInputs.menu = false;
        }
        if (_starterAssetsInputs.menu && !_interact.IsInteracting)
        {
            _starterAssetsInputs.menu = false;
            bool activeMenu = Menu.activeSelf;
            _Pause(activeMenu);
        }
    }

    private void _Pause(bool activeMenu)
    {
        if (!activeMenu)
        {
            Pause1.SetActive(true);
            Pause2.SetActive(false);
            Pause3.SetActive(false);
            Pause4.SetActive(false);
        }
        Menu.SetActive(!activeMenu);
        _eventSystem.firstSelectedGameObject = Button;
        _eventSystem.SetSelectedGameObject(Button);
        Cursor.visible = !activeMenu;
        _FPS.enabled = activeMenu;

        switch (activeMenu)
        {
            case false:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case true:
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                break;
        }
    }
}
