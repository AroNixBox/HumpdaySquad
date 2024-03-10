using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDenyManager : MonoBehaviour
{
    [SerializeField] private AudioClip denySound;
    public static TeleportDenyManager Instance { get; private set; }
    private AudioSource _audioSource;
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
        _audioSource = GetComponent<AudioSource>();
    }

    private List<TeleportDeny> _teleportDenyList = new List<TeleportDeny>();

    public void Add(TeleportDeny teleportDeny)
    {
        _teleportDenyList.Add(teleportDeny);
    }

    public bool CanTeleport()
    {
        foreach (var teleportDeny in _teleportDenyList)
        {
            if (!teleportDeny.CanTeleport)
            {
                _audioSource.PlayOneShot(denySound);
                return false;
            }
        }
        return true;
    }
}
