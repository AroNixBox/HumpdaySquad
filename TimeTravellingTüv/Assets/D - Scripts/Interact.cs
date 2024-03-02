using System;
using StarterAssets;
using UnityEngine;
public class Interact : MonoBehaviour
{
    private StarterAssetsInputs _starterAssetsInputs;
    private PhysicalClipboard _physicalClipboard;
    public bool IsInteracting;

    private void Awake()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        _physicalClipboard = FindObjectOfType<PhysicalClipboard>();
    }

    private void Update()
    {
        if(_physicalClipboard.IsClipboardEquipped) return;
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
        }
    }
}
public interface IInteraction
{
    void Interacter();
}
