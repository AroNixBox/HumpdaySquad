using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    void Interacter();
}

public class Interact : MonoBehaviour
{
    public bool IsInteracting;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsInteracting)
        {
            interact();
        }
    }

    public void interact()
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
