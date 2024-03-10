using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class Interact : MonoBehaviour
{
    private StarterAssetsInputs _starterAssetsInputs;
    private PhysicalClipboard _physicalClipboard;
    private PlayerInteractUI _playerInteractUI;
    private PlayerInit _playerInit;
    public bool isInteracting;
    private void Awake()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        _physicalClipboard = FindObjectOfType<PhysicalClipboard>();
        _playerInit = FindObjectOfType<PlayerInit>();
        _playerInteractUI = FindObjectOfType<PlayerInteractUI>();
        
        _starterAssetsInputs.OnInteractEvent += InteractWithObject;
    }

    private void InteractWithObject()
    {
        if(isInteracting) { return; }
        
        var distance = 2f;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObject))
            {
                interactObject.Interact(transform);
            }
        }
    }

    private void Update()
    {
        if(_playerInit.IsTeleporting) return;
        if (_physicalClipboard.IsClipboardEquipped)
        { 
            _playerInteractUI.Hide();
            return;
        }
        //Can interact with objects
        var distance = 2f;
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObject))
            {
                _playerInteractUI.Show();
            }
            else
            {
                _playerInteractUI.Hide();
            }
        }
        else
        {
            _playerInteractUI.Hide();
        }
    }

    private void OnDisable()
    {
        _starterAssetsInputs.OnInteractEvent -= InteractWithObject;
    }
}
public interface IInteractable
{
    void Interact(Transform playerTransform);
}
