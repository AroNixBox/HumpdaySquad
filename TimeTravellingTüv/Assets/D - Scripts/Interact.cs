using System;
using StarterAssets;
using UnityEngine;
public class Interact : MonoBehaviour
{
    private StarterAssetsInputs _starterAssetsInputs;
    private PhysicalClipboard _physicalClipboard;
    private PlayerInteractUI _playerInteractUI;
    private PlayerInit _playerInit;
    public bool IsInteracting;

    private void Awake()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        _physicalClipboard = FindObjectOfType<PhysicalClipboard>();
        _playerInit = FindObjectOfType<PlayerInit>();
        _playerInteractUI = FindObjectOfType<PlayerInteractUI>();
    }

    private void Update()
    {
        if(_playerInit.IsTeleporting) return;
        
        if (_physicalClipboard.IsClipboardEquipped)
        {
            _starterAssetsInputs.interact = false;
            _playerInteractUI.Hide();
            return;
        }
        //Can interact with objects
        float Distance = 2f;
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Distance))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteraction interactObject))
            {
                _playerInteractUI.Show();
                if (_starterAssetsInputs.interact && !IsInteracting)
                {
                    _starterAssetsInputs.interact = false;
                    interactObject.Interacter();
                    return;
                }
            }
            else if (hitInfo.collider.gameObject.TryGetComponent(out ITalkable interactableNPC))
            {
                _playerInteractUI.Show();
                if (_starterAssetsInputs.interact && !IsInteracting)
                {
                    _starterAssetsInputs.interact = false;
                    interactableNPC.Interact(transform);
                    return;
                }
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
}
public interface IInteraction
{
    void Interacter();
}
