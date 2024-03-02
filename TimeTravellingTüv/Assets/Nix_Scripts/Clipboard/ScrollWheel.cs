using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class ScrollWheel : MonoBehaviour
{
    private int _selectedIndex = 0;
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private UIClipboard uiClipboard;
    private IEnumerator Start()
    {
        yield return null;
        //highlight the first slot
        uiClipboard.HighlightSelectedSlot(_selectedIndex);
    }

    private void Update()
    {
        ProcessScrollInput();

        if (input.mark)
        {
            ProcessMarkInput();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"You got: {uiClipboard.DocumentPoints()} Points");
        }
    }

    private void ProcessScrollInput()
    {
        int previousIndex = _selectedIndex;
        float scroll = input.scroll;
        if (scroll == 0f) return;

        _selectedIndex += scroll > 0f ? -1 : 1;
        _selectedIndex = (_selectedIndex + uiClipboard.Entries.Count) % uiClipboard.Entries.Count;
        input.scroll = 0;
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