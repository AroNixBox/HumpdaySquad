using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIClipboard : MonoBehaviour
{
    [SerializeField] private GameObject clipboardEntryPrefab;
    public List<ClipboardEntry> Entries { get; } = new();
    private ClipboardEntry _selectedEntry;
    void Start()
    {
        for(int i = 0; i < Clipboard.Instance.ClipBoardEntries.Count; i++)
        {
            var entry = Instantiate(clipboardEntryPrefab, transform).GetComponent<ClipboardEntry>();
            Entries.Add(entry);
            entry.Init(Clipboard.Instance.ClipBoardEntries[i]);
        }
    }

    public void HighlightSelectedSlot(int selectedIndex)
    {
        for(int i = 0; i < Entries.Count; i++)
        {
            Entries[i].SetOutline(i == selectedIndex);
            
            if(i == selectedIndex)
            {
                _selectedEntry = Entries[i];
            }
        }
    }

    public int DocumentPoints()
    {
        //TODO: Handle hand in logic
        
        int points = 0;
        foreach (var entry in Entries)
        {
            //If I got the points for this entry
            if (Clipboard.Instance.CorrectEntries(entry.Status, Entries.IndexOf(entry)))
            {
                points++;
            }
        }

        return points;
    }
}
