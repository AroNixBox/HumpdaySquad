using UnityEngine;

public class TeleportDeny : MonoBehaviour
{
    public bool CanTeleport { get; private set; } = true;

    private void Start()
    {
        TeleportDenyManager.Instance.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanTeleport = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanTeleport = true;
        }
    }
}
