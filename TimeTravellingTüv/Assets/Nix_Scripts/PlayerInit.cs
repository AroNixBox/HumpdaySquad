using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    [SerializeField] private Transform playerCapsuleTransform;
    [SerializeField] private Transform playerCameraRootTransform;
    [SerializeField] private FirstPersonController firstPersonController;
    private void Start()
    {
        LoadPlayerStateAndSetPosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.Instance.ChangeScene(playerCapsuleTransform.position, playerCameraRootTransform.rotation, playerCapsuleTransform.rotation);
        }
    }

    private void LoadPlayerStateAndSetPosition()
    {
        (Vector3 playerCapsulePosition, Quaternion cameraRootRotation, Quaternion playerCapsuleRotation) = SceneManager.Instance.LoadPlayerState();
        
        //Nothing to load
        if(playerCapsulePosition == Vector3.zero && cameraRootRotation == Quaternion.identity && playerCapsuleRotation == Quaternion.identity) { return; }

        GetComponent<CharacterController>().Move(playerCapsulePosition);
        firstPersonController.InitializeCameraRotation(cameraRootRotation, playerCapsuleRotation);
    }
}
