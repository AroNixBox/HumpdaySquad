using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInit : MonoBehaviour
{
    [SerializeField] private Transform playerCapsuleTransform;
    [SerializeField] private Transform playerCameraRootTransform;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private ParticleSystem tpVFX;
    private CharacterController _characterController;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        LoadPlayerStateAndSetPosition();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)){ return; }
        if(!firstPersonController.Grounded) { return; }
        
        tpVFX.Play();
        SceneManager.Instance.ChangeScene(playerCapsuleTransform.position, playerCameraRootTransform.rotation, playerCapsuleTransform.rotation);
    }

    private void LoadPlayerStateAndSetPosition()
    {
        (Vector3 playerCapsulePosition, Quaternion cameraRootRotation, Quaternion playerCapsuleRotation) = SceneManager.Instance.LoadPlayerState();
        
        //Nothing to load
        if(playerCapsulePosition == Vector3.zero && cameraRootRotation == Quaternion.identity && playerCapsuleRotation == Quaternion.identity) { return; }

        _characterController.Move(playerCapsulePosition);
        firstPersonController.InitializeCameraRotation(cameraRootRotation, playerCapsuleRotation);
        tpVFX.Play();
    }
}
