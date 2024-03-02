using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class ScrollWheel : MonoBehaviour
{
    private int _selectedIndex = 0;
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private UIClipboard uiClipboard;
    private Interact _interact;

    private void Awake()
    {
        _interact = FindObjectOfType<Interact>();
    }

    private IEnumerator Start()
    {
        yield return null;
        //highlight the first slot
        uiClipboard.HighlightSelectedSlot(_selectedIndex);
    }

    private void Update()
    {
        //Cant interact with Checklist when interacting with other objects
        if (_interact.IsInteracting) return;
        
        //TODO: add a pullout to the Checklist
        
        ProcessScrollInput();

        if (input.mark)
        {
            ProcessMarkInput();
        }
    }

    private void ProcessScrollInput()
    {
        int previousIndex = _selectedIndex;
        float scroll = input.scroll;
        if (scroll == 0f) return;

        _selectedIndex += scroll > 0f ? -1 : 1;
        _selectedIndex = (_selectedIndex + uiClipboard.Entries.Count) % uiClipboard.Entries.Count;
        //input.scroll = 0;
        if (previousIndex != _selectedIndex)
        {
            uiClipboard.HighlightSelectedSlot(_selectedIndex);
        }
    }
    
    private void ProcessMarkInput()
    {
        input.mark = false;
        var entryToMark = uiClipboard.Entries[_selectedIndex];
        if(entryToMark == null) return;
        
        entryToMark.OccupySlot();
    }
}