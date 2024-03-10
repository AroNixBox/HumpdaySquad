using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardEntry : MonoBehaviour
{
    //0 is unoccupied, 1 is checked, 2 is crossed
    private int _occupiedIndex;
    public ClipboardEntryStatus Status => (ClipboardEntryStatus) _occupiedIndex;

    [Header("References")] 
    [SerializeField] private TextMeshProUGUI bulletpoint;
    [SerializeField] private Image checkmark;
    [SerializeField] private Image crossmark;
    [SerializeField] private GameObject markTint;
    [SerializeField] private AudioClip markSound;
    [SerializeField] private AudioClip crossSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(string text)
    {
        bulletpoint.text = text;
    }
    public void SetOutline(bool active)
    {
        markTint.SetActive(active);
    }

    public void OccupySlot()
    {
        switch (_occupiedIndex)
        {
            //First ever occupy, since it was 0 before
            case 0:
                _occupiedIndex++;
                _audioSource.PlayOneShot(markSound);
                checkmark.gameObject.SetActive(true);
                crossmark.gameObject.SetActive(false);
                break;
            
            case 1:
                _occupiedIndex++;
                _audioSource.PlayOneShot(crossSound);
                checkmark.gameObject.SetActive(false);
                crossmark.gameObject.SetActive(true);
                break;
            
            case 2:
                _occupiedIndex = 0;
                crossmark.gameObject.SetActive(false);
                checkmark.gameObject.SetActive(false);
                break;
        }
    }
}
public enum ClipboardEntryStatus
{
    Unoccupied,
    Checked,
    Crossed
}
