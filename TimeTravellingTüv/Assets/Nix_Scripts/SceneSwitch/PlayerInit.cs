using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInit : MonoBehaviour
{
    private FirstPersonController _firstPersonController;
    [Header("References")]
    [SerializeField] private GameObject tpVFX;
    [SerializeField] private AudioSource tpAudioSource;
    
    [Header("Values")]
    [SerializeField] private AudioClip startTPClip;
    [SerializeField] private AudioClip endTPClip;
    private Interact _interact;
    
    private void Awake()
    {
        _firstPersonController = GetComponent<FirstPersonController>();
        _interact = FindObjectOfType<Interact>();
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)){ return; }
        if(!_firstPersonController.Grounded) { return; }
        if(_firstPersonController.JumpTimeoutDelta > 0f) { return; }
        if(_interact.IsInteracting) { return; }

        StartCoroutine(TeleportVFX());
    }
    private IEnumerator TeleportVFX()
    {
        tpVFX.SetActive(true);
        _firstPersonController.isPaused = true;
        
        tpAudioSource.clip = startTPClip;
        tpAudioSource.Play();
        yield return new WaitForSeconds(tpAudioSource.clip.length);
        
        SceneManager.Instance.ChangeLevel();
 
        tpAudioSource.clip = endTPClip;
        tpAudioSource.Play();
        yield return new WaitForSeconds(tpAudioSource.clip.length);
        
        _firstPersonController.isPaused = false;
        tpVFX.SetActive(false);
    }
}
