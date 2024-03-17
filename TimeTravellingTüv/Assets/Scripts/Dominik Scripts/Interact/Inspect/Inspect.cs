using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nix_Scripts.SceneSwitch;
using StarterAssets;
using UnityEngine;

public class Inspect : MonoBehaviour, IInteractable
{
    private Vector3 _inspectSize;
    private float objectLerpSpeed = 10f;
    [SerializeField] private float objDistance = 1.5f;
    
    private int inspectLayer = 3;
    private Dictionary<Transform, int> originalLayers;

    
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private bool _isInspecting;
    private bool _recentlyInspected;
    
    private GameObject _camera;
    private FirstPersonController _characterController;
    private StarterAssetsInputs _starterAssetsInputs;
    private PlayerInit _playerInit;
    private Interact _interact;
    private float _rotMod;
    private float _divisor;

    private void Awake()
    {
        _characterController = FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None).FirstOrDefault();
        _playerInit = FindObjectsByType<PlayerInit>(FindObjectsSortMode.None).FirstOrDefault();
        _camera = Camera.main.gameObject;
        _starterAssetsInputs = FindObjectsByType<StarterAssetsInputs>(FindObjectsSortMode.None).FirstOrDefault();
        _interact = FindObjectsByType<Interact>(FindObjectsSortMode.None).FirstOrDefault();
        
    }
    void Start()
    {
        _starterAssetsInputs.OnInteractEvent += EndInspect;
        _starterAssetsInputs.OnCancelEvent += EndInspect;
        Debug.Log(gameObject.name + "subscribed to OnModifiersChangedEvent");
        InspectReferenceManager.Instance.OnModifiersChangedEvent += ChangeModifiers;

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
        
        _inspectSize = GetInspectSize();
    }

    private void ChangeModifiers(float rotMod, float divisor)
    {
        Debug.Log("Modifiers changed");
        _rotMod = rotMod;
        _divisor = divisor;
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
        if(_playerInit.IsTeleporting) return;
        if(_recentlyInspected) { return; }

        _interact.isInteracting = true;

        _recentlyInspected = true;
        _characterController.enabled = false;
        _isInspecting = true;

        originalLayers = new Dictionary<Transform, int>();
        SetLayerRecursively(transform, inspectLayer, originalLayers);

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

        _interact.isInteracting = false;
        _isInspecting = false;
        _characterController.enabled = true;

        ResetLayerRecursively(transform, originalLayers);
    }
    private void ResetLayerRecursively(Transform trans, Dictionary<Transform, int> originalLayers)
    {
        if (originalLayers.ContainsKey(trans))
        {
            trans.gameObject.layer = originalLayers[trans];
        }
        foreach (Transform child in trans)
        {
            ResetLayerRecursively(child, originalLayers);
        }
    }

    private void SetLayerRecursively(Transform trans, int layer, Dictionary<Transform, int> originalLayers)
    {
        originalLayers[trans] = trans.gameObject.layer;
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
        {
            SetLayerRecursively(child, layer, originalLayers);
        }
    }
    private void InspectObject()
    {
        transform.position = Vector3.Lerp(transform.position, _camera.transform.position + _camera.transform.forward * objDistance, Time.deltaTime * 5f);

        float rotX = _starterAssetsInputs.look.x * _rotMod * Time.deltaTime;
        float rotY = -_starterAssetsInputs.look.y * _rotMod * Time.deltaTime;
        transform.Rotate(_camera.transform.up, -rotX, Space.World);
        transform.Rotate(_camera.transform.right, rotY, Space.World);
        
        float scrollInput = _starterAssetsInputs.scroll;
        var localScale = transform.localScale;
        Vector3 targetScale = localScale + Vector3.one * (scrollInput / _divisor);
        targetScale = new Vector3(
            Mathf.Clamp(targetScale.x, _inspectSize.x / 3, _inspectSize.x * 3),
            Mathf.Clamp(targetScale.y, _inspectSize.y / 3, _inspectSize.y * 3),
            Mathf.Clamp(targetScale.z, _inspectSize.z / 3, _inspectSize.z * 3));
        
        
        localScale = Vector3.Lerp(localScale, targetScale, Time.deltaTime * objectLerpSpeed);
        transform.localScale = localScale;
    }
    private Vector3 GetInspectSize()
    {
        var referenceCube = InspectReferenceManager.Instance.InspectReferenceCube;
        if (referenceCube == null)
        {
            Debug.LogError("Reference cube is not assigned!");
            return transform.localScale;
        }

        var combinedBounds = new Bounds(transform.position, Vector3.zero);
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        Vector3 referenceScale = referenceCube.transform.localScale;
        Vector3 objectScale = combinedBounds.size;

        float scaleFactorX = referenceScale.x / objectScale.x;
        float scaleFactorY = referenceScale.y / objectScale.y;
        float scaleFactorZ = referenceScale.z / objectScale.z;

        float scaleFactor = Mathf.Min(scaleFactorX, scaleFactorY, scaleFactorZ);

        Vector3 scale = Vector3.one * scaleFactor;

        // Überprüfen, ob die berechnete Skalierung gültig ist
        if (float.IsNaN(scale.x) || float.IsInfinity(scale.x))
        {
            Debug.LogWarning("Invalid scale calculated for object: " + gameObject.name);
            scale = Vector3.one;
        }

        return scale;
    }

    private void ReturnToOriginalState()
    {
        transform.position = Vector3.Lerp(transform.position, _originalPosition, Time.deltaTime * objectLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _originalRotation, Time.deltaTime * objectLerpSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, Time.deltaTime * objectLerpSpeed);
    }
    private void OnDestroy()
    {
        _starterAssetsInputs.OnInteractEvent -= EndInspect;
        _starterAssetsInputs.OnCancelEvent -= EndInspect;
        InspectReferenceManager.Instance.OnModifiersChangedEvent -= ChangeModifiers;
    }
}
