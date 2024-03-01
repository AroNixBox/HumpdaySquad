using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    [SerializeField] private Transform playerCapsuleTransform;
    [SerializeField] private Transform playerCameraRootTransform;
    private void Start()
    {
        LoadPlayerStateAndSetPosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.Instance.ChangeScene(playerCapsuleTransform.position, playerCameraRootTransform.rotation, "Nixon_Dev_ONE");
        }
    }

    private void LoadPlayerStateAndSetPosition()
    {
        (Vector3 position, Quaternion rotation) = SceneManager.Instance.LoadPlayerState();
        
        //Nothing to load
        if(position == Vector3.zero && rotation == Quaternion.identity) { return; }

        GetComponent<CharacterController>().Move(position);
        playerCameraRootTransform.rotation = rotation;
    }
}
