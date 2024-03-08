using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectReferenceManager : MonoBehaviour
{
    public static InspectReferenceManager Instance { get; private set; }
    [field: SerializeField] public GameObject InspectReferenceCube { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
