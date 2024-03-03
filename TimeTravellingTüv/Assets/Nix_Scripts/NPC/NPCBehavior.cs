using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour, ITalkable
{
    public void Interact(Transform playerHeadTransform)
    {
        Debug.Log("Interacting with NPC");
    }

}
