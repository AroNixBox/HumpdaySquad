using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDenyManager : MonoBehaviour
{
    public static TeleportDenyManager Instance { get; private set; }

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
                return false;
            }
        }
        return true;
    }
}
