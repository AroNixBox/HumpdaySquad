using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Pause1;
    [SerializeField] private GameObject Pause2;
    [SerializeField] private GameObject Pause3;
    [SerializeField] private GameObject Pause4;
    private FirstPersonController _FPS;
    private StarterAssetsInputs _starterAssetsInputs;
    private void Start()
    {
        _FPS = GetComponent<FirstPersonController>();
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_starterAssetsInputs.menu)
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
