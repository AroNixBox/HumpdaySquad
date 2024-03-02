using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInit : MonoBehaviour
{
    private FirstPersonController _firstPersonController;
    [SerializeField] private ParticleSystem tpVFX;
    
    private void Awake()
    {
        _firstPersonController = GetComponent<FirstPersonController>();
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)){ return; }
        if(!_firstPersonController.Grounded) { return; }
        if(_firstPersonController.JumpTimeoutDelta > 0f) { return; }
        
        _firstPersonController.isPaused = true;
        tpVFX.Play();
        SceneManager.Instance.ChangeLevel();
        _firstPersonController.isPaused = false;
    }
}
