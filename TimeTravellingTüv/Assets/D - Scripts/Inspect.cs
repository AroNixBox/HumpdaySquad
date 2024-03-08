using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public class Inspect : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 inspectSize = new (1f, 1f, 1f);
    [SerializeField] private float objDistance = 1.5f;
    
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private bool _isInspecting;
    private bool _recentlyInspected;

    private const float MouseRotSpeed = 500f;
    private const float ControllerRotSpeed = 1f;
    private const float ControllerDivisor = 100f;
    private const float MouseDivisor = 500f;
    private GameObject _camera;
    private FirstPersonController _characterController;
    private StarterAssetsInputs _starterAssetsInputs;
    private float _rotMod;
    private float _divisor;

    private void Awake()
    {
        _characterController = FindObjectOfType<FirstPersonController>();
        _camera = Camera.main.gameObject;
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        
        _starterAssetsInputs.OnInteractEvent += EndInspect;
    }

    private void OnDisable()
    {
        _starterAssetsInputs.OnInteractEvent -= EndInspect;
    }

    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
        UpdateRotModBasedOnInputDevice();
    }

    private void Update()
    {
        if (_isInspecting)
        {
            InspectObject();
        }
        else if (_originalPosition != transform.position || _originalRotation != transform.rotation || _originalScale != transform.localScale)
        {
            ReturnToOriginalState();
        }
    }

    public void Interact(Transform playerTransform)
    {
        if(_isInspecting) { return; }
        
        _recentlyInspected = true;
        _characterController.enabled = false;
        _isInspecting = true;

        StartCoroutine(StopRecentlyInspecting());
    }
    private IEnumerator StopRecentlyInspecting()
    {
        yield return new WaitForSeconds(.1f);
        _recentlyInspected = false;
    }
    private void EndInspect()
    {
        if(!_isInspecting) { return;}
        if(_recentlyInspected) { return; }
        
        _isInspecting = false;
        _characterController.enabled = true;
    }
    private void InspectObject()
    {
        transform.position = Vector3.Lerp(transform.position, _camera.transform.position + _camera.transform.forward * objDistance, Time.deltaTime * 5f);

        float rotX = _starterAssetsInputs.look.x * _rotMod * Time.deltaTime;
        float rotY = -_starterAssetsInputs.look.y * _rotMod * Time.deltaTime;
        transform.Rotate(_camera.transform.up, -rotX, Space.World);
        transform.Rotate(_camera.transform.right, rotY, Space.World);
        
        float scrollInput = _starterAssetsInputs.scroll;
        Vector3 targetScale = transform.localScale + Vector3.one * (scrollInput / _divisor);
        transform.localScale = new Vector3(
            Mathf.Clamp(targetScale.x, inspectSize.x / 3, inspectSize.x * 3),
            Mathf.Clamp(targetScale.y, inspectSize.y / 3, inspectSize.y * 3),
            Mathf.Clamp(targetScale.z, inspectSize.z / 3, inspectSize.z * 3));
    }

    private void ReturnToOriginalState()
    {
        transform.position = Vector3.Lerp(transform.position, _originalPosition, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, _originalRotation, Time.deltaTime * 5f);
        transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, Time.deltaTime * 5f);
    }

    private void UpdateRotModBasedOnInputDevice()
    {
        _rotMod = IsControllerConnected() ? ControllerRotSpeed : MouseRotSpeed;
        _divisor = IsControllerConnected() ? ControllerDivisor : MouseDivisor;
    }

    private bool IsControllerConnected()
    {
        var joysticks = Input.GetJoystickNames();
        return joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]);
    }

    private void LateUpdate()
    {
        UpdateRotModBasedOnInputDevice();
    }
}
