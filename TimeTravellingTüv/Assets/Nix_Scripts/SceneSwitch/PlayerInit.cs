using System;
using System.Collections;
using System.Collections.Generic;
using Nix_Scripts.SceneSwitch;
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
    public bool IsTeleporting { get; private set; }
    private StarterAssetsInputs _input;
    private Interact _interact;
    
    private void Awake()
    {
        _firstPersonController = GetComponent<FirstPersonController>();
        _interact = FindObjectOfType<Interact>();
        _input = FindObjectOfType<StarterAssetsInputs>();
    }
    private void Update()
    {
        if (!_input.teleport) { return; }
        _input.teleport = false;
        
        if(!_firstPersonController.Grounded) { return; }
        if(_firstPersonController.JumpTimeoutDelta > 0f) { return; }
        if(_interact.IsInteracting) { return; }

        StartCoroutine(TeleportVFX());
    }
    private IEnumerator TeleportVFX()
    {
        IsTeleporting = true;
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
        IsTeleporting = false;
    }
}
