using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Clipboard : MonoBehaviour
{
    //Add enough space for each string to be stored
    [field: SerializeField, TextArea(3, 10)] public List<string> ClipBoardEntries {get; private set;} = new();
    public static Clipboard Instance { get; private set; }
    [Tooltip("CORRECT: STANDARTS WERE MET, COSSED: STANDARTS WERE NOT MET (THIS CAUSED THE ISSUE)")]
    [SerializeField] private ClipboardEntryStatus[] clipboardEntryStatus;
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
    public bool CorrectEntries(ClipboardEntryStatus passedInStatus, int index)
    {
        //Has the player set the status to Unoccupied and the standards were met anyways
        if(passedInStatus == ClipboardEntryStatus.Unoccupied && clipboardEntryStatus[index] == ClipboardEntryStatus.Checked)
        {
            //still return the entry true
            return true;
        }
        
        return clipboardEntryStatus[index] == passedInStatus;
    }

    public void GetClipboardResults()
    {
        
    }
}
