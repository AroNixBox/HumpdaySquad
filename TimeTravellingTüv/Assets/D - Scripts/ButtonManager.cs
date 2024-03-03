using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Menu;
    private FirstPersonController _FPS;

    private void Start()
    {
        _FPS = Player.GetComponent<FirstPersonController>();
    }
    public void Continue()
    {
        Time.timeScale = 1.0f;
        _FPS.enabled = true;
        Menu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
