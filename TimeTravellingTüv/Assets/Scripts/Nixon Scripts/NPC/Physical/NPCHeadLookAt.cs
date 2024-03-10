using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadLookAt : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;
    private Transform _cameraTransform;
    private bool _isLookingAtPosition;
    private Transform _lookAtLock;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_lookAtLock == null) { return; }
        
        if((transform.position - _cameraTransform.position).sqrMagnitude > 100f)
        {
            //Too far away
            _isLookingAtPosition = false;
        }
        
        headLookAtTransform.position = _lookAtLock.position;
        float targetweight = _isLookingAtPosition ? 1.0f : 0.0f;
        float lerpSpeed = 2f;
        rig.weight = Mathf.Lerp(rig.weight, targetweight, Time.deltaTime * lerpSpeed);
    }
    
    public void LookAtPosition(Transform lookAtPosition)
    {
        _isLookingAtPosition = true;
        _lookAtLock = lookAtPosition;
        
        //TODO: Do this when conversation ends
    }
}
