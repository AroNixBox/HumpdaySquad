using System;
using StarterAssets;
using UnityEngine;
public class Interact : MonoBehaviour
{
    private StarterAssetsInputs _starterAssetsInputs;
    public bool IsInteracting;

    private void Awake()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    private void Update()
    {
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
