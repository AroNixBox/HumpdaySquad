using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StarterAssets;


public class Inspect : MonoBehaviour, IInteraction
{
    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _scale;
    private bool _inspecting;
    private bool _returning;
    private bool _startInspecting;
    private bool _stopInspecting;

    private float _divisor;
    private float _rotMod;

    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for rotating object along local x-axis")]
    [SerializeField] private float RotX;
    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for rotating object along local y-axis")]
    [SerializeField] private float RotY;
    [Tooltip("(Value for the rotation-speed of the object")]
    [SerializeField] private float RotSpeed = 2f;

    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for the amount of zooming happening")]
    [SerializeField] private float Zoom;
    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for the maximum size the object can become")]
    [SerializeField] private Vector3 MaxSize;
    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for the minimum size the object can become")]
    [SerializeField] private Vector3 MinSize;
    
    private GameObject _camera;
    
    private FirstPersonController _characterController;
    private StarterAssetsInputs _starterAssetsInputs;
    private Interact _interact;

    [Tooltip("Size the object will inherit when inspecting (needed for large objects (needs to be manually added")]
    public Vector3 InspectSize;

    private void Awake()
    {
        _characterController = FindObjectOfType<FirstPersonController>();
        _camera = Camera.main.gameObject;
        _interact = Camera.main.GetComponent<Interact>();
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    void Start()
    {
        UpdateDivisorBasedOnInputDevice();
        _position = transform.position;
        _rotation = transform.rotation;
        _scale = transform.localScale;

        MaxSize = InspectSize * 5f;
        MinSize = InspectSize / 5f;
    }

    private void Update()
    {
        if (_inspecting)
        {
            _interact.IsInteracting = true;
            _characterController.enabled = false;

            RotX = -_starterAssetsInputs.look.x / _rotMod;
            RotY = -_starterAssetsInputs.look.y / _rotMod; 
            float targetZoom = _starterAssetsInputs.scroll * transform.localScale.x / _divisor;
            Zoom = Mathf.Lerp(Zoom, targetZoom, .1f);

            if (transform.position != _camera.transform.position + _camera.transform.forward * 1.5f)
            {
                transform.position = Vector3.Lerp(transform.position, _camera.transform.position
                + _camera.transform.forward * 1.5f, 0.1f);
            }

            if (_startInspecting) // Change rotation and scale of object to inspect values at start of inspecting
            {
                if (transform.localScale != InspectSize)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, InspectSize, 0.5f);
                }

                if (transform.position == _camera.transform.position + _camera.transform.forward * 1.5f ||
                    transform.localScale == InspectSize || transform.rotation == _camera.transform.rotation)
                {
                    _startInspecting = false;
                }
            }

            transform.rotation = Quaternion.AngleAxis(RotX * RotSpeed, transform.up) *
                                 Quaternion.AngleAxis(RotY * RotSpeed, transform.right) *
                                 transform.rotation;
            
            if (_starterAssetsInputs.resetInspect) // This will set the rotation and scale of the object to default inspect
            {
                _starterAssetsInputs.resetInspect = false;

                 transform.rotation = _camera.transform.rotation;
                 transform.localScale = InspectSize;
            }
            
            // Zoom in and out of an object
            transform.localScale = new Vector3(
                Mathf.Clamp(transform.localScale.x + Zoom, MinSize.x, MaxSize.x), 
                Mathf.Clamp(transform.localScale.y + Zoom, MinSize.y, MaxSize.y), 
                Mathf.Clamp(transform.localScale.z + Zoom, MinSize.z, MaxSize.z));
        }

        if (_stopInspecting)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || _starterAssetsInputs.interact || _starterAssetsInputs.cancelAction)
            {
                _starterAssetsInputs.interact = false;
                _starterAssetsInputs.cancelAction = false;


                ReturnToDefault();
                _stopInspecting = false;
                StartCoroutine(EnableInteract());
            }
        }
        if (_returning) // Set Transform of object to state before inspect
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, 
                    _position, 
                    0.5f), 
                Quaternion.Lerp(
                    transform.rotation, 
                    _rotation, 
                    0.5f));
            
            transform.localScale = Vector3.Lerp(transform.localScale, _scale, 0.5f);

            if (transform.position == _position || transform.rotation == _rotation
                || transform.localScale == _scale)
            {
                _returning = false;
            }
        }
    }

    public void Interacter()
    {
        _startInspecting = true;
        _inspecting = true;
        transform.rotation = Quaternion.Lerp(transform.rotation, _camera.transform.rotation, 0.5f);
        _stopInspecting = true;
    }

    private IEnumerator EnableInteract() 
    {
        yield return new WaitForSeconds(0.1f);
        _interact.IsInteracting = false;
    }

    void ReturnToDefault()
    {
        _returning = true;
        _inspecting = false;
        _characterController.enabled = true;
    }
    void UpdateDivisorBasedOnInputDevice()
    {
        if (IsControllerConnected())
        {
            _divisor = 100f; // Controller angeschlossen
            _rotMod = 100f;
        }
        else
        {
            _divisor = 500f; // Kein Controller, Maus wird verwendet
            _rotMod = 1f;
        }
    }

    bool IsControllerConnected()
    {
        string[] joysticks = Input.GetJoystickNames();
        foreach (string joystick in joysticks)
        {
            if (!string.IsNullOrEmpty(joystick))
            {
                return true; // Mindestens ein Controller ist angeschlossen
            }
        }
        return false; // Keine Controller angeschlossen
    }
}
