using System;
using StarterAssets;
using UnityEngine;
public class Interact : MonoBehaviour
{
    private StarterAssetsInputs _starterAssetsInputs;
    private PhysicalClipboard _physicalClipboard;
    private PlayerInit _playerInit;
    public bool IsInteracting;

    private void Awake()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        _physicalClipboard = FindObjectOfType<PhysicalClipboard>();
        _playerInit = FindObjectOfType<PlayerInit>();
    }

    private void Update()
    {
        if(_playerInit.IsTeleporting) return;
        
        if (_physicalClipboard.IsClipboardEquipped)
        {
            _starterAssetsInputs.interact = false;
            return;
        }
        if (_starterAssetsInputs.interact && !IsInteracting)
        {
            _starterAssetsInputs.interact = false;
            interact();
        }
    }

    private void interact()
    {
        float Distance = 2f;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Distance))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteraction interactObject))
            {
                interactObject.Interacter();
            }
            else if (hitInfo.collider.gameObject.TryGetComponent(out ITalkable interactableNPC))
            {
                interactableNPC.Interact(transform);
            }
        }
    }
}
public interface IInteraction
{
    void Interacter();
}
