using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public class Inspect : MonoBehaviour, IInteractable
{
    private Vector3 _inspectSize;
    private float objectLerpSpeed = 10f;
    [SerializeField] private float objDistance = 1.5f;
    
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private bool _isInspecting;
    private bool _recentlyInspected;

    private const float MouseRotSpeed = 500f;
    private const float ControllerRotSpeed = 1f;
    private const float ControllerDivisor = 1f;
    private const float MouseDivisor = 10f;
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
    }

    private void OnDisable()
    {
        _starterAssetsInputs.OnInteractEvent -= EndInspect;
    }

    void Start()
    {
        _starterAssetsInputs.OnInteractEvent += EndInspect;

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
        UpdateRotModBasedOnInputDevice();
        
        _inspectSize = GetInspectSize();
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
        if(_recentlyInspected) { return; }
        
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
        targetScale = new Vector3(
            Mathf.Clamp(targetScale.x, _inspectSize.x / 3, _inspectSize.x * 3),
            Mathf.Clamp(targetScale.y, _inspectSize.y / 3, _inspectSize.y * 3),
            Mathf.Clamp(targetScale.z, _inspectSize.z / 3, _inspectSize.z * 3));

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * objectLerpSpeed);
    }
    private Vector3 GetInspectSize()
    {
        var referenceCube = InspectReferenceManager.Instance.InspectReferenceCube;
        if(referenceCube == null)
        {
            Debug.LogError("Reference cube is not assigned!");
            return new Vector3(1, 1, 1);
        }

        var combinedBounds = new Bounds(transform.position, Vector3.zero);
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }
    
        var scaleFactor = Mathf.Min(
            referenceCube.transform.localScale.x / combinedBounds.size.x,
            referenceCube.transform.localScale.y / combinedBounds.size.y,
            referenceCube.transform.localScale.z / combinedBounds.size.z);

        return new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private void ReturnToOriginalState()
    {
        transform.position = Vector3.Lerp(transform.position, _originalPosition, Time.deltaTime * objectLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _originalRotation, Time.deltaTime * objectLerpSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, Time.deltaTime * objectLerpSpeed);
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
