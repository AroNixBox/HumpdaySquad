using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ControllerCheck : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private string _controlScheme;
    [SerializeField] private GameObject eSystem;
    [SerializeField] private GameObject Button;
    private EventSystem _system;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _system = eSystem.GetComponent<EventSystem>();
        _controlScheme = _playerInput.currentControlScheme;
    }

    private void Update()
    {
        if (_playerInput.currentControlScheme != _controlScheme) 
        {
            _system.firstSelectedGameObject = Button;
            _system.SetSelectedGameObject(Button);
            _controlScheme = _playerInput.currentControlScheme;
        }
    }
}
