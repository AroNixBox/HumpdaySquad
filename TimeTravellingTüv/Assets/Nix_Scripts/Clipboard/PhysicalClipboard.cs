using System;
using StarterAssets;
using UnityEngine;

public class PhysicalClipboard : MonoBehaviour
{
    [SerializeField] private Transform unequippedLerpQlipboardParent;
    [SerializeField] private Transform equippedLerpQlipboardParent;
    private Interact _interact;

    public bool IsClipboardEquipped { get; private set; }
    private StarterAssetsInputs _input;

    private void Awake()
    {
        _input = FindObjectOfType<StarterAssetsInputs>();
        _interact = FindObjectOfType<Interact>();
    }

    private void Update()
    {
        if (_interact.IsInteracting)
        {
            _input.checklistPull = false;
            return;
        }
        if (!_interact.IsInteracting && _input.checklistPull || 
            !_interact.IsInteracting && IsClipboardEquipped && _input.cancelAction)
        {
            _input.checklistPull = false; _input.cancelAction = false;
            IsClipboardEquipped = !IsClipboardEquipped;
        }

        var parent = IsClipboardEquipped ? equippedLerpQlipboardParent : unequippedLerpQlipboardParent;
        transform.position = Vector3.Lerp(transform.position, parent.position, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, 10f * Time.deltaTime);
    }
}
