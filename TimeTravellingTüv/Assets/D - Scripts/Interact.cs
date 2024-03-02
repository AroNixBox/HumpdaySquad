using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    void Interacter();
}

public class Interact : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
